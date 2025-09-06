# ---------- Build stage ----------
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy only the project file first (faster + catches name/path errors early)
COPY MapTheMuseApi.csproj ./

# Restore dependencies
RUN dotnet restore ./MapTheMuseApi.csproj

# Now copy the rest of the source
COPY . ./

# Publish (no app host, Release)
RUN dotnet publish ./MapTheMuseApi.csproj -c Release -o /app/publish /p:UseAppHost=false

# ---------- Runtime stage ----------
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
ENV ASPNETCORE_URLS=http://0.0.0.0:8080
EXPOSE 8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "MapTheMuseApi.dll"]
