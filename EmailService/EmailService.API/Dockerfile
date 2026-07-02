FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shared/NotificationSolution.MessageBroker.Abstraction/NotificationSolution.MessageBroker.Abstraction.csproj", "Shared/NotificationSolution.MessageBroker.Abstraction/"]
COPY ["Shared/NotificationSolution.MessageBroker.Kafka/NotificationSolution.MessageBroker.Kafka.csproj", "Shared/NotificationSolution.MessageBroker.Kafka/"]
COPY ["EmailService/EmailService.API/EmailService.API.csproj", "EmailService/EmailService.API/"]
COPY ["EmailService/EmailService.Infrastructure/EmailService.Infrastructure.csproj", "EmailService/EmailService.Infrastructure/"]
COPY ["EmailService/EmailService.Application/EmailService.Application.csproj", "EmailService/EmailService.Application/"]
COPY ["EmailService/EmailService.Domain/EmailService.Domain.csproj", "EmailService/EmailService.Domain/"]
RUN dotnet restore "EmailService/EmailService.API/EmailService.API.csproj"
COPY . .
WORKDIR "/src/EmailService/EmailService.API"
RUN dotnet build "./EmailService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmailService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailService.API.dll"]
