#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:7.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/MarketDepth.Pub/MarketDepth.Pub.csproj", "src/MarketDepth.Pub/"]
RUN dotnet restore "src/MarketDepth.Pub/MarketDepth.Pub.csproj"
COPY . .
WORKDIR "/src/src/MarketDepth.Pub"
RUN dotnet build "MarketDepth.Pub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketDepth.Pub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketDepth.Pub.dll"]
