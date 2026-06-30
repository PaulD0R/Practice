FROM mcr.microsoft.com/dotnet/runtime:10.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["EmailService.API/EmailService.API.csproj", "EmailService.API/"]
COPY ["SmsService.Infrastructure/SmsService.Infrastructure.csproj", "SmsService.Infrastructure/"]
COPY ["SmsService.Application/SmsService.Application.csproj", "SmsService.Application/"]
COPY ["SmsService.Domain/SmsService.Domain.csproj", "SmsService.Domain/"]
RUN dotnet restore "EmailService.API/EmailService.API.csproj"
COPY . .
WORKDIR "/src/EmailService.API"
RUN dotnet build "./EmailService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./EmailService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EmailService.API.dll"]
