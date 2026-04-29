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
        var response = await _httpClient.PostAsJsonAsync(
            $"{_baseUrl}/ia/bmc/generate",
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

        return await response.Content.ReadFromJsonAsync<object>() ?? throw new Exception("Respuesta de IA vacía");
    }
}