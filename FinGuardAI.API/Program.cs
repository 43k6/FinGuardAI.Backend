using FinGuardAI.Business.Services;
using FinGuardAI.DataAccess.Persistence;
using FinGuardAI.DataAccess.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using WMS.Infrastructure.Persistence.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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

app.UseAuthorization();

app.MapControllers();

app.Run();
