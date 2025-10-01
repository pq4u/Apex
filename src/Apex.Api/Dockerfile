FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Apex.Api/Apex.Api.csproj", "src/Apex.Api/"]
COPY ["src/Apex.Application/Apex.Application.csproj", "src/Apex.Application/"]
COPY ["src/Apex.Domain/Apex.Domain.csproj", "src/Apex.Domain/"]
COPY ["src/Apex.Infrastructure/Apex.Infrastructure.csproj", "src/Apex.Infrastructure/"]
RUN dotnet restore "src/Apex.Api/Apex.Api.csproj"
COPY . .
WORKDIR "/src/src/Apex.Api"
RUN dotnet build "./Apex.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Apex.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Apex.Api.dll"]
