# Etapa de compilación
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# Copiar archivos de solución y restaurar dependencias
COPY *.sln ./
COPY src/EmprendeIA.Domain/*.csproj ./src/EmprendeIA.Domain/
COPY src/EmprendeIA.Application/*.csproj ./src/EmprendeIA.Application/
COPY src/EmprendeIA.Infrastructure/*.csproj ./src/EmprendeIA.Infrastructure/
COPY src/EmprendeIA.Api/*.csproj ./src/EmprendeIA.Api/
RUN dotnet restore

# Copiar todo el código y publicar
COPY . ./
RUN dotnet publish src/EmprendeIA.Api/EmprendeIA.Api.csproj -c Release -o out

# Etapa de ejecución en runtime ligero
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /app/out .

# Configurar puertos de escucha
ENV ASPNETCORE_URLS=http://+:5244
EXPOSE 5244

ENTRYPOINT ["dotnet", "EmprendeIA.Api.dll"]