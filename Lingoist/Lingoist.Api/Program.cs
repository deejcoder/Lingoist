using Lingoist.Backend.Application.Features.TextToSpeech;
using Lingoist.Backend.Providers.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddRequiredProviderServices();

string elevenLabsApiKey = builder.Configuration["ElevenLabs:ApiKey"] ?? throw new ApplicationException("No API key provided for Eleven Labs");
builder.Services.AddTextToSpeech(elevenLabsApiKey);

builder.Services.AddTransient<TextToSpeechService>();


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
