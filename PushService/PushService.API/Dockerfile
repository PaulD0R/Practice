FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shared/NotificationSolution.MessageBroker.Abstraction/NotificationSolution.MessageBroker.Abstraction.csproj", "Shared/NotificationSolution.MessageBroker.Abstraction/"]
COPY ["Shared/NotificationSolution.MessageBroker.Kafka/NotificationSolution.MessageBroker.Kafka.csproj", "Shared/NotificationSolution.MessageBroker.Kafka/"]
COPY ["Shared/NotificationSolution.MessageBroker.Rabbit/NotificationSolution.MessageBroker.Rabbit.csproj", "Shared/NotificationSolution.MessageBroker.Rabbit/"]
COPY ["PushService/PushService.API/PushService.API.csproj", "PushService/PushService.API/"]
COPY ["PushService/PushService.Infrastructure/PushService.Infrastructure.csproj", "PushService/PushService.Infrastructure/"]
COPY ["PushService/PushService.Application/PushService.Application.csproj", "PushService/PushService.Application/"]
COPY ["PushService/PushService.Domain/PushService.Domain.csproj", "PushService/PushService.Domain/"]
RUN dotnet restore "PushService/PushService.API/PushService.API.csproj"
COPY . .
WORKDIR "/src/PushService/PushService.API"
RUN dotnet build "./PushService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./PushService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "PushService.API.dll"]
