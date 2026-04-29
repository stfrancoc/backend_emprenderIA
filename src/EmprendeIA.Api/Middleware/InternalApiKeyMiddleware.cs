using System.Security.Cryptography;
using System.Text;

namespace EmprendeIA.Api.Middleware;

public class InternalApiKeyMiddleware
{
    private const string ApiKeyHeaderName = "X-API-KEY";
    private const string InternalAiPathPrefix = "/internal/ai";

    private readonly RequestDelegate _next;
    private readonly IConfiguration _configuration;

    public InternalApiKeyMiddleware(RequestDelegate next, IConfiguration configuration)
    {
        _next = next;
        _configuration = configuration;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (!context.Request.Path.StartsWithSegments(InternalAiPathPrefix, StringComparison.OrdinalIgnoreCase))
        {
            await _next(context);
            return;
        }

        var expectedApiKey = _configuration["InternalApi:ApiKey"];

        if (string.IsNullOrWhiteSpace(expectedApiKey))
        {
            await WriteUnauthorizedAsync(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(ApiKeyHeaderName, out var providedApiKey))
        {
            await WriteUnauthorizedAsync(context);
            return;
        }

        if (!AreEqualInConstantTime(providedApiKey.ToString(), expectedApiKey))
        {
            await WriteUnauthorizedAsync(context);
            return;
        }

        await _next(context);
    }

    private static bool AreEqualInConstantTime(string provided, string expected)
    {
        var providedBytes = Encoding.UTF8.GetBytes(provided);
        var expectedBytes = Encoding.UTF8.GetBytes(expected);

        if (providedBytes.Length != expectedBytes.Length)
        {
            return false;
        }

        return CryptographicOperations.FixedTimeEquals(providedBytes, expectedBytes);
    }

    private static async Task WriteUnauthorizedAsync(HttpContext context)
    {
        context.Response.StatusCode = StatusCodes.Status401Unauthorized;
        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(new { error = "Unauthorized" });
    }
}