FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ChromeHistoryAnalyzer/ChromeHistoryAnalyzer.csproj", "ChromeHistoryAnalyzer/"]
RUN dotnet restore "ChromeHistoryAnalyzer/ChromeHistoryAnalyzer.csproj"
COPY . .
WORKDIR "/src/ChromeHistoryAnalyzer"
RUN dotnet build "./ChromeHistoryAnalyzer.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./ChromeHistoryAnalyzer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChromeHistoryAnalyzer.dll"]
