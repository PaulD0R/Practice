FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shared/NotificationSolution.MessageBroker.Abstraction/NotificationSolution.MessageBroker.Abstraction.csproj", "Shared/NotificationSolution.MessageBroker.Abstraction/"]
COPY ["Shared/NotificationSolution.MessageBroker.Kafka/NotificationSolution.MessageBroker.Kafka.csproj", "Shared/NotificationSolution.MessageBroker.Kafka/"]
COPY ["NotificationService/NotificationService.API/NotificationService.API.csproj", "NotificationService/NotificationService.API/"]
COPY ["NotificationService/NotificationService.Infrastructure/NotificationService.Infrastructure.csproj", "NotificationService/NotificationService.Infrastructure/"]
COPY ["NotificationService/NotificationService.Application/NotificationService.Application.csproj", "NotificationService/NotificationService.Application/"]
COPY ["NotificationService/NotificationService.Domain/NotificationService.Domain.csproj", "NotificationService/NotificationService.Domain/"]
COPY ["NotificationService/NotificationService.Tests/NotificationService.Tests.csproj", "NotificationService/NotificationService.Tests/"]
RUN dotnet restore "NotificationService/NotificationService.API/NotificationService.API.csproj"
COPY . .
RUN dotnet test "./NotificationService/NotificationService.Tests/NotificationService.Tests.csproj" --configuration Release
WORKDIR "/src/NotificationService/NotificationService.API"
RUN dotnet build "./NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./NotificationService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "NotificationService.API.dll"]
