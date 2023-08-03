FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7777

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source
COPY ["StockApi/Stock.RestAPI.csproj", "StockApi/"]
COPY ["Stock.Business/Stock.Business.csproj", "Stock.Business/"]
COPY ["Stock.Capabilities/Stock.Capabilities.csproj", "Stock.Capabilities/"]
COPY ["Stock.Domain/Stock.Domain.csproj", "Stock.Domain/"]
COPY ["Stock.Messaging.Kafka/Stock.Messaging.Kafka.csproj", "Stock.Messaging.Kafka/"]
COPY ["Stock.Persistence.EFCore/Stock.Persistence.EFCore.csproj", "Stock.Persistence.EFCore/"]
COPY ["Stock.Querying.EFCore/Stock.Querying.EFCore.csproj", "Stock.Querying.EFCore/"]
COPY ["Stock.Supporting/Stock.Supporting.csproj", "Stock.Supporting/"]
COPY ["Protos/*.proto", "Protos/"]

RUN dotnet restore "StockApi/Stock.RestAPI.csproj"
COPY . .
WORKDIR "/source/StockApi"
RUN dotnet build "Stock.RestAPI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Stock.RestAPI.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_URLS="http://localhost:7777"
ENTRYPOINT ["dotnet", "Stock.RestApi.dll"]
