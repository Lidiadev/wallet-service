FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Wallet.API/Wallet.API.csproj", "Wallet.API/"]
COPY ["Wallet.Domain/Wallet.Domain.csproj", "Wallet.Domain/"]
COPY ["Wallet.Application/Wallet.Application.csproj", "Wallet.Application/"]
COPY ["Wallet.Infrastructure/Wallet.Infrastructure.csproj", "Wallet.Infrastructure/"]

RUN dotnet restore "Wallet.API/Wallet.API.csproj"
COPY . .
WORKDIR "/src/Wallet.API"
RUN dotnet build "Wallet.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Wallet.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS final
WORKDIR /app
COPY --from=publish /app/publish .

RUN dotnet tool install --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"

COPY --from=build /. /.
ENTRYPOINT dotnet ef database update --project /src/Wallet.Infrastructure/Wallet.Infrastructure.csproj && dotnet Wallet.API.dll
