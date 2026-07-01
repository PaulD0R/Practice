FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PushService.API/PushService.API.csproj", "PushService.API/"]
COPY ["PushService.Infrastructure/PushService.Infrastructure.csproj", "PushService.Infrastructure/"]
COPY ["PushService.Application/PushService.Application.csproj", "PushService.Application/"]
COPY ["PushService.Domain/PushService.Domain.csproj", "PushService.Domain/"]
RUN dotnet restore "PushService.API/PushService.API.csproj"
COPY . .
WORKDIR "/src/PushService.API"
RUN dotnet build "./PushService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PushService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PushService.API.dll"]
