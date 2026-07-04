FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Shared/NotificationSolution.MessageBroker.Abstraction/NotificationSolution.MessageBroker.Abstraction.csproj", "Shared/NotificationSolution.MessageBroker.Abstraction/"]
COPY ["Shared/NotificationSolution.MessageBroker.Kafka/NotificationSolution.MessageBroker.Kafka.csproj", "Shared/NotificationSolution.MessageBroker.Kafka/"]
COPY ["Shared/NotificationSolution.MessageBroker.Rabbit/NotificationSolution.MessageBroker.Rabbit.csproj", "Shared/NotificationSolution.MessageBroker.Rabbit/"]
COPY ["SmsService/SmsService.API/SmsService.API.csproj", "SmsService/SmsService.API/"]
COPY ["SmsService/SmsService.Infrastructure/SmsService.Infrastructure.csproj", "SmsService/SmsService.Infrastructure/"]
COPY ["SmsService/SmsService.Application/SmsService.Application.csproj", "SmsService/SmsService.Application/"]
COPY ["SmsService/SmsService.Domain/SmsService.Domain.csproj", "SmsService/SmsService.Domain/"]
RUN dotnet restore "SmsService/SmsService.API/SmsService.API.csproj"
COPY . .
WORKDIR "/src/SmsService/SmsService.API"
RUN dotnet build "./SmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmsService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmsService.API.dll"]
