FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 7777

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Ecommerce.ConsumerProductChangelog/Ecommerce.ConsumerProductChangelog.csproj", "Ecommerce.ConsumerProductChangelog/"]
RUN dotnet restore "Ecommerce.ConsumerProductChangelog/Ecommerce.ConsumerProductChangelog.csproj"
COPY . .
WORKDIR "/src/Ecommerce.ConsumerProductChangelog"
RUN dotnet build "Ecommerce.ConsumerProductChangelog.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Ecommerce.ConsumerProductChangelog.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .

ENV DOTNET_EnableDiagnostics=0
ENV ASPNETCORE_URLS="http://localhost:7777"
ENTRYPOINT ["dotnet", "Ecommerce.ConsumerProductChangelog.dll"]
