#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IvNav.Store.Identity.Web/IvNav.Store.Identity.Web.csproj", "IvNav.Store.Identity.Web/"]
COPY ["IvNav.Store.Identity.Core/IvNav.Store.Identity.Core.csproj", "IvNav.Store.Identity.Core/"]
COPY ["IvNav.Store.Common/IvNav.Store.Common.csproj", "IvNav.Store.Common/"]
COPY ["IvNav.Store.Core.Interaction/IvNav.Store.Core.Interaction.csproj", "IvNav.Store.Core.Interaction/"]
COPY ["IvNav.Store.Enums/IvNav.Store.Enums.csproj", "IvNav.Store.Enums/"]
COPY ["IvNav.Store.Identity.Infrastructure/IvNav.Store.Identity.Infrastructure.csproj", "IvNav.Store.Identity.Infrastructure/"]
COPY ["IvNav.Store.Infrastructure/IvNav.Store.Infrastructure.csproj", "IvNav.Store.Infrastructure/"]
COPY ["IvNav.Store.Setup/IvNav.Store.Setup.csproj", "IvNav.Store.Setup/"]
RUN dotnet restore "IvNav.Store.Identity.Web/IvNav.Store.Identity.Web.csproj"
COPY . .
WORKDIR "/src/IvNav.Store.Identity.Web"
RUN dotnet build "IvNav.Store.Identity.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IvNav.Store.Identity.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IvNav.Store.Identity.Web.dll"]