#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IvNav.Store.Product.Web/IvNav.Store.Product.Web.csproj", "IvNav.Store.Product.Web/"]
COPY ["IvNav.Store.Product.Core/IvNav.Store.Product.Core.csproj", "IvNav.Store.Product.Core/"]
COPY ["IvNav.Store.Product.Infrastructure/IvNav.Store.Product.Infrastructure.csproj", "IvNav.Store.Product.Infrastructure/"]
COPY ["IvNav.Store.Infrastructure/IvNav.Store.Infrastructure.csproj", "IvNav.Store.Infrastructure/"]
COPY ["IvNav.Store.Common/IvNav.Store.Common.csproj", "IvNav.Store.Common/"]
COPY ["IvNav.Store.Setup/IvNav.Store.Setup.csproj", "IvNav.Store.Setup/"]
RUN dotnet restore "IvNav.Store.Product.Web/IvNav.Store.Product.Web.csproj"
COPY . .
WORKDIR "/src/IvNav.Store.Product.Web"
RUN dotnet build "IvNav.Store.Product.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IvNav.Store.Product.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IvNav.Store.Product.Web.dll"]