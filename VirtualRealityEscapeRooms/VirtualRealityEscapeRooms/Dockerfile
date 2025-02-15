FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["VirtualRealityEscapeRooms.csproj", "."]
RUN dotnet restore "./VirtualRealityEscapeRooms.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "VirtualRealityEscapeRooms.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "VirtualRealityEscapeRooms.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENV ASPNETCORE_URLS=http://*:80
ENV ASPNETCORE_ENVIRONMENT=Development
ENTRYPOINT ["dotnet", "VirtualRealityEscapeRooms.dll"]