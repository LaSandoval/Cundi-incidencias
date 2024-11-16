# Establece la imagen base para el entorno de ejecución de .NET
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80

# Usar la imagen del SDK de .NET para compilar y publicar la aplicación
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["Cundi-incidencias/Cundi-incidencias.csproj", "Cundi-incidencias/"]
RUN dotnet restore "Cundi-incidencias/Cundi-incidencias.csproj"
COPY . .
WORKDIR "/src/Cundi-incidencias"
RUN dotnet build "Cundi-incidencias.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Cundi-incidencias.csproj" -c Release -o /app/publish

# Crea la imagen final para ejecutar la aplicación
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Cundi-incidencias.dll"]
