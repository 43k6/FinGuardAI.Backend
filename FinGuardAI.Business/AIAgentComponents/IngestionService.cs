namespace API_AI_Agent.Component
{
    using Microsoft.Extensions.Configuration;
    using Microsoft.SemanticKernel.Embeddings;
    using Microsoft.VisualBasic;
    using PDFtoImage;
    using Pinecone;
    using SkiaSharp;
    using System.Net.Http.Headers;
    using System.Text;
    using System.Text.Json;
    using static System.Net.Mime.MediaTypeNames;

    public class IngestionService
    {
        private readonly PineconeClient _pinecone;
        private readonly string _indexName;
        private readonly ITextEmbeddingGenerationService _embeddingService;
        private readonly string _openAiApiKey; // نحتاج المفتاح لإرسال الصور

        public IngestionService(IConfiguration config, ITextEmbeddingGenerationService embeddingService)
        {
            _openAiApiKey = config["OpenAIApiKey"] ?? config["OpenAI:ApiKey"]; // تأكد من اسم المتغير في appsettings
            string apiKey = config["PineconeApiKey"] ?? config["Pinecone_Key"];
            _indexName = config["PineconeIndexName"] ?? config["Pinecone_Index"];

            _pinecone = new PineconeClient(apiKey);
            _embeddingService = embeddingService;
        }

        public async Task IngestPdfAsync(string filePath)
        {
            Console.WriteLine("1. 👁️ جاري تشغيل محرك الرؤية البصري (GPT-4o Vision)...");

            // استخراج النص عبر الذكاء الاصطناعي البصري
            string fullText = await ExtractTextViaVisionAsync(filePath);

            if (string.IsNullOrWhiteSpace(fullText) || fullText.Length < 10)
            {
                Console.WriteLine("⚠️ تحذير: فشل استخراج النص أو الملف فارغ.");
                return;
            }

            Console.WriteLine("2. 📦 جاري تقطيع النص النظيف لرفع البيانات...");
            var chunks = SplitTextIntoChunks(fullText, maxChunkSize: 400); // كبرنا الحجم قليلاً لأن النص نظيف جداً

            using var index = await _pinecone.GetIndex(_indexName);
            var vectors = new List<Vector>();

            for (int i = 0; i < chunks.Count; i++)
            {
                if (string.IsNullOrWhiteSpace(chunks[i])) continue;

                var embedding = await _embeddingService.GenerateEmbeddingAsync(chunks[i]);

                vectors.Add(new Vector
                {
                    Id = $"doc-{Guid.NewGuid()}-chunk-{i}",
                    Values = embedding.ToArray(),
                    Metadata = new MetadataMap { { "text", chunks[i] } }
                });
            }

            if (vectors.Count > 0)
            {
                await index.Upsert(vectors);
                Console.WriteLine($"🚀 تم رفع {vectors.Count} متجه (Vectors) بدقة 100% إلى Pinecone!");
            }
        }

        private async Task<string> ExtractTextViaVisionAsync(string filePath)
        {
            var sb = new StringBuilder();

            try
            {
                Console.WriteLine("📸 جاري تحويل الـ PDF لصور باستخدام محرك SkiaSharp الحديث...");
                var base64Images = new List<string>();

                // 1. تحويل الـ PDF إلى صور بشكل آمن تماماً (Cross-Platform)
                byte[] pdfBytes = await File.ReadAllBytesAsync(filePath);
                var pageImages = PDFtoImage.Conversion.ToImages(pdfBytes); // من مكتبة PDFtoImage

                foreach (var skBitmap in pageImages)
                {
                    // تحويل الـ Bitmap إلى Jpeg عالي الدقة ثم إلى Base64
                    using var image = SKImage.FromBitmap(skBitmap);
                    using var data = image.Encode(SKEncodedImageFormat.Jpeg, 100); // دقة 100%
                    using var ms = new MemoryStream();
                    data.SaveTo(ms);
                    base64Images.Add(Convert.ToBase64String(ms.ToArray()));
                }

                Console.WriteLine($"✅ تم إنشاء {base64Images.Count} صور. جاري إرسالها لـ OpenAI...");

                // 2. إرسال الصور لـ GPT-4o (نفس الكود السابق تماماً)
                using var client = new HttpClient();
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _openAiApiKey);

                for (int i = 0; i < base64Images.Count; i++)
                {
                    var requestBody = new
                    {
                        model = "gpt-4o",
                        messages = new[]
                        {
                    new
                    {
                        role = "user",
                        content = new object[]
                        {
                            new { type = "text", text = "أنت خبير في تدقيق المستندات. استخرج النص العربي والأرقام والبنود من هذه الصورة بدقة 100%. لا تقم بإضافة أي مقدمات أو شروحات، فقط اكتب النص الموجود." },
                            new { type = "image_url", image_url = new { url = $"data:image/jpeg;base64,{base64Images[i]}" } }
                        }
                    }
                },
                        max_tokens = 2000
                    };

                    var content = new StringContent(JsonSerializer.Serialize(requestBody), Encoding.UTF8, "application/json");
                    var response = await client.PostAsync("https://api.openai.com/v1/chat/completions", content);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseString = await response.Content.ReadAsStringAsync();
                        using var jsonDoc = JsonDocument.Parse(responseString);
                        var extractedText = jsonDoc.RootElement.GetProperty("choices")[0].GetProperty("message").GetProperty("content").GetString();

                        sb.AppendLine(extractedText);
                        Console.WriteLine($"✅ تمت قراءة الصفحة {i + 1} بواسطة Vision API!");
                    }
                    else
                    {
                        Console.WriteLine($"❌ خطأ من OpenAI API: {await response.Content.ReadAsStringAsync()}");
                    }
                }

                File.WriteAllText("Vision_Clean_Debug.txt", sb.ToString(), Encoding.UTF8);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ خطأ فادح: {ex.Message}");
            }

            return sb.ToString();
        }

        private List<string> SplitTextIntoChunks(string text, int maxChunkSize)
        {
            var chunks = new List<string>();
            var lines = text.Split(new[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            var currentChunk = new StringBuilder();

            foreach (var line in lines)
            {
                if (currentChunk.Length + line.Length > maxChunkSize && currentChunk.Length > 0)
                {
                    chunks.Add(currentChunk.ToString().Trim());
                    currentChunk.Clear();
                }
                currentChunk.AppendLine(line);
            }
            if (currentChunk.Length > 0) chunks.Add(currentChunk.ToString().Trim());
            return chunks;
        }
    }
}