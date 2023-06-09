﻿FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

RUN mkdir Database

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["SetGame.csproj", "./"]
RUN dotnet restore "SetGame.csproj"
COPY . .
WORKDIR "/src/"
RUN dotnet build "SetGame.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "SetGame.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SetGame.dll"]
