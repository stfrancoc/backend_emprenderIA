using System.Net.Http.Json;
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
        var response = await _httpClient.PostAsJsonAsync($"{_baseUrl}/ia/bmc/generate", input);
        
        if (!response.IsSuccessStatusCode)
            throw new Exception("Error al conectar con el microservicio de IA");

        return await response.Content.ReadFromJsonAsync<object>() ?? throw new Exception("Respuesta de IA vacía");
    }
}