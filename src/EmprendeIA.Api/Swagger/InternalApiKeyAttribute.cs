namespace EmprendeIA.Api.Swagger;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
public sealed class InternalApiKeyAttribute : Attribute
{
}