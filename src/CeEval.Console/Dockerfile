#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CeEval.Console/CeEval.Console.csproj", "src/CeEval.Console/"]
COPY ["src/CeEval.Shared/CeEval.Shared.csproj", "src/CeEval.Shared/"]
RUN dotnet restore "src/CeEval.Console/CeEval.Console.csproj"
COPY . .
WORKDIR "/src/src/CeEval.Console"
RUN dotnet build "CeEval.Console.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CeEval.Console.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CeEval.Console.dll"]