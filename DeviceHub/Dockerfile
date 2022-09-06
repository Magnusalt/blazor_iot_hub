FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DeviceHub/DeviceHub.csproj", "DeviceHub/"]
RUN dotnet restore "DeviceHub/DeviceHub.csproj"
COPY . .
WORKDIR "/src/DeviceHub"
RUN dotnet build "DeviceHub.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeviceHub.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceHub.dll"]
