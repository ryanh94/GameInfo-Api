FROM mcr.microsoft.com/dotnet/core/aspnet:3.0-buster-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:3.0-buster AS build
WORKDIR /src
COPY ["GameInfo.API/GameInfo.API.csproj", "GameInfo.API/"]
RUN dotnet restore "GameInfo.API/GameInfo.API.csproj"
COPY . .
WORKDIR "/src/GameInfo.API"
RUN dotnet build "GameInfo.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "GameInfo.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "GameInfo.API.dll"]