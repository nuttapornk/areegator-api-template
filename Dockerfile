#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 80
ENV ASPNETCORE_HTTP_PORTS 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/_NTLCOMPONENT_/_NTLCOMPONENT_.csproj", "src/_NTLCOMPONENT_/"]
COPY ["src/Application/Application.csproj", "src/Application/"]
COPY ["src/Domain/Domain.csproj", "src/Domain/"]
COPY ["src/Infrastructure/Infrastructure.csproj", "src/Infrastructure/"]
RUN dotnet restore "./src/_NTLCOMPONENT_/_NTLCOMPONENT_.csproj"
COPY . .
WORKDIR "/src/src/_NTLCOMPONENT_"
RUN dotnet build "./_NTLCOMPONENT_.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./_NTLCOMPONENT_.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "_NTLPLATFORM_._NTLDOMAIN_._NTLCOMPONENT_.dll"]