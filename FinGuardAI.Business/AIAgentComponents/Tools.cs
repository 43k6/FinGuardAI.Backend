namespace API_AI_Agent.Component
{
    using Microsoft.SemanticKernel;
    using System.ComponentModel;
    using Pinecone;
    using Microsoft.Extensions.Configuration;
    using System.Linq;
    using Microsoft.SemanticKernel.Embeddings;

    public class KnowledgeBasePlugin
    {
        private readonly PineconeClient _pinecone;
        private readonly string _indexName;
        private readonly ITextEmbeddingGenerationService _embeddingService;

        public KnowledgeBasePlugin(IConfiguration config, ITextEmbeddingGenerationService embeddingService)
        {
            string apiKey = config["PineconeApiKey"] ?? config["Pinecone_Key"];
            _indexName = config["PineconeIndexName"] ?? config["Pinecone_Index"];

            _pinecone = new PineconeClient(apiKey);
            _embeddingService = embeddingService;
        }

        [KernelFunction("SearchDatabase"), Description("Searches the internal knowledge base for technical info and documentation. Use this when you need to find information from uploaded PDF documents.")]
        public async Task<string> SearchKnowledgeBaseAsync([Description("The search query to look up in the knowledge base")] string query)
        {
            // 1. طباعة السؤال لنعرف ماذا يطلب الذكاء الاصطناعي من قاعدة البيانات
            Console.WriteLine($"\n🔍 الوكيل (Agent) يبحث الآن في Pinecone عن: '{query}'");

            try
            {
                using var index = await _pinecone.GetIndex(_indexName);

                float[] queryEmbedding = await GetEmbeddingForQuery(query);

                // جلب أفضل 5 نتائج
                var results = await index.Query(queryEmbedding, topK: 5, includeMetadata: true);

                if (results == null || !results.Any())
                {
                    Console.WriteLine("❌ Pinecone لم يجد أي تطابق!");
                    return "No relevant information found in the knowledge base.";
                }

                Console.WriteLine($"✅ تم سحب {results.Count()} مربعات نصية. جاري كشفها للوكيل...");

                var validChunks = new List<string>();
                foreach (var r in results)
                {
                    // 2. الحل السحري: استخدام الـ Casting (string) لاستخراج النص من الـ Heap بدلاً من ToString()
                    string chunkText = "";
                    if (r.Metadata != null && r.Metadata.ContainsKey("text"))
                    {
                        // تحويل آمن ومباشر للنوع
                        // 2. الحل السحري: استخراج النص من داخل هيكل Pinecone

                        if (r.Metadata != null && r.Metadata.ContainsKey("text"))
                        {
                            // نطلب الخاصية Inner التي تحتوي على النص الحقيقي
                            chunkText = r.Metadata["text"].Inner?.ToString() ?? "";
                        }
                    }

                    if (!string.IsNullOrWhiteSpace(chunkText))
                    {
                        validChunks.Add(chunkText);
                        // 3. طباعة الـ Score وأول 60 حرف لكي تتأكد بنفسك من جودة التطابق
                        Console.WriteLine($"[Score: {r.Score:F2}] -> {chunkText.Substring(0, Math.Min(60, chunkText.Length)).Replace('\n', ' ')}...");
                    }
                }

                // إرجاع النصوص النظيفة للوكيل لكي يصيغ الإجابة
                return string.Join("\n\n---\n\n", validChunks);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطأ في البحث: {ex.Message}");
                return "There is an error retrieving the data from the database.";
            }
        }

        private async Task<float[]> GetEmbeddingForQuery(string text)
        {
            var embedding = await _embeddingService.GenerateEmbeddingAsync(text);
            return embedding.ToArray();
        }
    }
}