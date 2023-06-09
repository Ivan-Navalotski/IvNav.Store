#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IvNav.Store.Web/IvNav.Store.Web.csproj", "IvNav.Store.Web/"]
COPY ["IvNav.Store.Core/IvNav.Store.Core.csproj", "IvNav.Store.Core/"]
COPY ["IvNav.Store.Common/IvNav.Store.Common.csproj", "IvNav.Store.Common/"]
COPY ["IvNav.Store.Infrastructure/IvNav.Store.Infrastructure.csproj", "IvNav.Store.Infrastructure/"]
COPY ["IvNav.Store.Setup/IvNav.Store.Setup.csproj", "IvNav.Store.Setup/"]
RUN dotnet restore "IvNav.Store.Web/IvNav.Store.Web.csproj"
COPY . .
WORKDIR "/src/IvNav.Store.Web"
RUN dotnet build "IvNav.Store.Web.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "IvNav.Store.Web.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IvNav.Store.Web.dll"]
