using System.Net.Http.Json;
using System.Text.Json;
using EmprendeIA.Domain.Interfaces;
using Microsoft.Extensions.Configuration;

namespace EmprendeIA.Infrastructure.Services;

public class AiService : IAIService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl;

    public AiService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _baseUrl = configuration["AiService:BaseUrl"] ?? "http://localhost:8000";
    }

    public async Task<object> GenerateBmcAsync(object input)
    {
        return await PostToAiAsync("/ia/bmc/generate", input);
    }

    public async Task<object> GenerateFinancialAnalysisAsync(object input)
    {
        return await PostToAiAsync("/ia/financial/generate", input);
    }

    public async Task<object> ChatAsync(object input)
    {
        return await PostToAiAsync("/ia/assistant/chat", input);
    }

    public async Task<ProductClassifyResponse?> ClassifyProductAsync(string name, string description)
    {
        var payload = new { name, description };
        var result = await PostToAiAsync("/api/ai/classify-product", payload);
        if (result == null) return null;

        var jsonElement = (JsonElement)result;
        return JsonSerializer.Deserialize<ProductClassifyResponse>(
            jsonElement.GetRawText(),
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
    }

    public async Task<CalculateMatchesResponse?> CalculateMatchesAsync(string projectId, string bmcText)
    {
        var payload = new { project_id = projectId, bmc_text = bmcText };
        var result = await PostToAiAsync("/ia/matching/calculate", payload);
        if (result == null) return null;

        var jsonElement = (JsonElement)result;
        return JsonSerializer.Deserialize<CalculateMatchesResponse>(
            jsonElement.GetRawText(),
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower
            });
    }

    private async Task<object> PostToAiAsync(string path, object input)
    {
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}{path}",
            input,
            new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.SnakeCaseLower,
                DictionaryKeyPolicy = JsonNamingPolicy.SnakeCaseLower
            });

        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new Exception($"Error al conectar con el microservicio de IA ({(int)response.StatusCode}): {errorBody}");
        }

        var doc = await response.Content.ReadFromJsonAsync<JsonElement?>();
        if (doc == null) throw new Exception("Respuesta de IA vacía");
        return doc.Value;
    }
}