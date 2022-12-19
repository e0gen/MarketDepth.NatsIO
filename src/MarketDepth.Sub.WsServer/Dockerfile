#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5050/
EXPOSE 80
EXPOSE 443
EXPOSE 5050

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["src/MarketDepth.Sub.WsServer/MarketDepth.Sub.WsServer.csproj", "src/MarketDepth.Sub.WsServer/"]
RUN dotnet restore "src/MarketDepth.Sub.WsServer/MarketDepth.Sub.WsServer.csproj"
COPY . .
WORKDIR "/src/src/MarketDepth.Sub.WsServer"
RUN dotnet build "MarketDepth.Sub.WsServer.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MarketDepth.Sub.WsServer.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MarketDepth.Sub.WsServer.dll"]