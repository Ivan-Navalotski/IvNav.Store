#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["IvNav.Store.Web.Report/IvNav.Store.Web.Mail.csproj", "IvNav.Store.Web.Mail/"]
RUN dotnet restore "IvNav.Store.Web.Mail/IvNav.Store.Web.Mail.csproj"
COPY . .
WORKDIR "/src/IvNav.Store.Web.Mail"
RUN dotnet build "IvNav.Store.Web.Mail.csproj" -c Debug -o /app/build

FROM build AS publish
RUN dotnet publish "IvNav.Store.Web.Mail.csproj" -c Debug -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IvNav.Store.Web.Mail.dll"]
