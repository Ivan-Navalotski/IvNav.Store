#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IvNav.Store.Mail.Web/IvNav.Store.Mail.Web.csproj", "IvNav.Store.Mail.Web/"]
COPY ["IvNav.Store.Mail.Core/IvNav.Store.Mail.Core.csproj", "IvNav.Store.Mail.Core/"]
COPY ["IvNav.Store.Setup/IvNav.Store.Setup.csproj", "IvNav.Store.Setup/"]
COPY ["IvNav.Store.Common/IvNav.Store.Common.csproj", "IvNav.Store.Common/"]
COPY ["IvNav.Store.Core/IvNav.Store.Core.csproj", "IvNav.Store.Core/"]
COPY ["IvNav.Store.Core.Interaction/IvNav.Store.Core.Interaction.csproj", "IvNav.Store.Core.Interaction/"]
COPY ["IvNav.Store.Infrastructure/IvNav.Store.Infrastructure.csproj", "IvNav.Store.Infrastructure/"]
RUN dotnet restore "IvNav.Store.Mail.Web/IvNav.Store.Mail.Web.csproj"
COPY . .
WORKDIR "/src/IvNav.Store.Mail.Web"
RUN dotnet build "IvNav.Store.Mail.Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "IvNav.Store.Mail.Web.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IvNav.Store.Mail.Web.dll"]