using System.Reflection;
using Microsoft.AspNetCore.Authorization;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace EmprendeIA.Api.Swagger;

public sealed class SecurityRequirementsOperationFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        if (context.MethodInfo.GetCustomAttribute<InternalApiKeyAttribute>(inherit: true) != null ||
            context.MethodInfo.DeclaringType?.GetCustomAttribute<InternalApiKeyAttribute>(inherit: true) != null)
        {
            AddSecurityRequirement(operation, "X-API-KEY");
            return;
        }

        if (HasAuthorizeAttribute(context.MethodInfo))
        {
            AddSecurityRequirement(operation, "Bearer");
        }
    }

    private static bool HasAuthorizeAttribute(MethodInfo methodInfo)
    {
        return methodInfo.GetCustomAttribute<AuthorizeAttribute>(inherit: true) != null ||
               methodInfo.DeclaringType?.GetCustomAttribute<AuthorizeAttribute>(inherit: true) != null;
    }

    private static void AddSecurityRequirement(OpenApiOperation operation, string schemeId)
    {
        operation.Security ??= new List<OpenApiSecurityRequirement>();

        operation.Security.Add(new OpenApiSecurityRequirement
        {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeId
                    }
                },
                Array.Empty<string>()
            }
        });
    }
}