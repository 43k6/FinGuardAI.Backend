namespace API_AI_Agent.Component
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.SemanticKernel;
    using Microsoft.SemanticKernel.ChatCompletion;
    using Microsoft.SemanticKernel.Connectors.OpenAI; 
    using System.Collections.Concurrent;
    using System;

    public class AgentService
    {
        private readonly Kernel _kernel;
        private readonly IChatCompletionService _chatCompletion;
        private readonly ConcurrentDictionary<string, ChatHistory> _sessions = new();

        public AgentService(IConfiguration config, KnowledgeBasePlugin pineconePlugin)
        {
            var builder = Kernel.CreateBuilder();

            builder.AddOpenAIChatCompletion(
                modelId: "gpt-4o-mini",
                apiKey: config["OpenAiApiKey"] ?? config["OpenAI_ApiKey"]
            );

            builder.Plugins.AddFromObject(pineconePlugin, "KnowledgeBase");

            _kernel = builder.Build();
            _chatCompletion = _kernel.GetRequiredService<IChatCompletionService>();
        }

        public async Task<string> RunAgentAsync(string sessionId, string message)
        {
            var history = _sessions.GetOrAdd(sessionId, _ =>
            new ChatHistory(@"أنت محلل مالي ذكي وخبير تقني مخصص لتحليل المستندات والسياسات المرفوعة حصراً.
            لديك أداة 'SearchDatabase' للوصول إلى قاعدة بيانات المتجهات (Vector Database).

            ⚠️ القواعد المقدسة للرد:
            1. البحث الإلزامي: أي استفسار عن طلب مالي، مشروع، أو متطلب، يجب أن يبدأ باستخدام أداة 'SearchDatabase'.
            2. الالتزام بالسياق: إجابتك يجب أن تستند بنسبة 100% على النصوص المسترجعة من الأداة. 
            3. التعامل مع الغموض والطلبات الاستثنائية (الراية الصفراء):
               - إذا كان السؤال خارج نطاق سياسات العمل والملفات المرفوعة تماماً، قل بوضوح: 'عذراً، هذه المعلومة غير متوفرة في الوثائق المرفوعة حالياً'.
               - إياك أن تصدر قراراً بالرفض القاطع لأي طلب مالي يخص أعمال الشركة مهما بدا مخالفاً للسياسة. مهمتك هي 'التحليل والتوجيه' فقط. استخدم دائماً هذا القالب الإلزامي في ردك عند تحليل أي طلب مالي:
                 * البند المقترح: [اذكر اسم البند ورقم المادة دقيقاً].
                 * التوصية: [توجيه للمراجعة اليدوية من قبل المدير المالي].
                 * التبرير: [اشرح المخالفة أو الغموض بناءً على المستند، واترك قرار الرفض أو القبول للمدير المالي].
            4. الدقة المطلقة في أرقام المواد: عند الاستشهاد بأي سياسة، يجب عليك قراءة واستخراج (رقم المادة) و(اسمها) بشكل دقيق جداً ومطابق للنص المسترجع (مثال: 'بناءً على المادة 14: حظر التجزئة'). إياك أن تخمن رقم المادة أو تستشهد بمادة في غير سياقها.
            5. منع المعرفة العامة: إياك أن تجيب من معلوماتك التدريبية العامة. التزم بالسياسات المرفوعة فقط.
            6. الدقة المالية: انقل الأرقام المالية وحدود الصلاحيات كما وردت في المستند تماماً.
            7. الاحترافية والصياغة: كن مباشراً في إجابتك، لا تشرح خطوات عملك الداخلية، وقم بصياغة الإجابة بلغة عربية فصحى، سليمة، ومهنية، لتكون القراءة سلسة للمدير المالي."));

            history.AddUserMessage(message);

            var executionSettings = new OpenAIPromptExecutionSettings
            {
                FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(), 
                Temperature = 0.0 
            };

            try
            {
                // 4. هنا سيقوم الوكيل بقراءة السؤال، ثم إيقاف المحادثة، والذهاب لـ Pinecone، جلب البيانات، ثم صياغة الرد
                var response = await _chatCompletion.GetChatMessageContentAsync(history, executionSettings, _kernel);

                history.AddAssistantMessage(response.Content ?? string.Empty);

                string safeContent = response.Content ?? "";
                Console.WriteLine($"\n✅ إجابة الوكيل: {safeContent.Substring(0, Math.Min(safeContent.Length, 100))}...");

                return response.Content ?? "No response generated.";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Error in runAgent: {ex.Message}");
                throw;
            }
        }
    }
}