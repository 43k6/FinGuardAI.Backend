using API_AI_Agent.Component;
using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.Persistence;
using FinGuardAI.DataAccess.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.SemanticKernel;
using WMS.Infrastructure.Persistence.Repositories;
Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.InputEncoding = System.Text.Encoding.UTF8;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers()
    .AddJsonOptions(options =>
    {
        // هذا السطر يمنع السيرفر من تشفير الحروف العربية في استجابة Swagger
        options.JsonSerializerOptions.Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping;
    });

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AI Agent Service

builder.Services.AddCors();
builder.Services.AddSingleton<AgentService>();
builder.Services.AddScoped<IngestionService>();
builder.Services.AddTransient<KnowledgeBasePlugin>();

string openAiApiKey = builder.Configuration["OpenAiApiKey"];

builder.Services.AddOpenAITextEmbeddingGeneration(
    modelId: "text-embedding-3-small",
    apiKey: openAiApiKey,
    dimensions: 1024 
);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration["ConnectionString"]));


builder.Services.AddScoped<PersonRepository>();
builder.Services.AddScoped<PersonService>();
builder.Services.AddScoped<FinancialRequestRepository>();
builder.Services.AddScoped<FinancialRequestService>();
builder.Services.AddScoped<FinancialResponseRepository>();
builder.Services.AddScoped<FinancialResponseService>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// CORS must be before Authorization
app.UseCors(policy => policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

app.UseAuthorization();

app.MapControllers();

app.Run();
