#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["src/CeEval.Web/CeEval.Web.csproj", "src/CeEval.Web/"]
COPY ["src/CeEval.Shared/CeEval.Shared.csproj", "src/CeEval.Shared/"]
RUN dotnet restore "src/CeEval.Web/CeEval.Web.csproj"
COPY . .
WORKDIR "/src/src/CeEval.Web"
RUN dotnet build "CeEval.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CeEval.Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CeEval.Web.dll"]