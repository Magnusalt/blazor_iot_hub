FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["DeviceSimulator/DeviceSimulator.csproj", "DeviceSimulator/"]
RUN dotnet restore "DeviceSimulator/DeviceSimulator.csproj"
COPY . .
WORKDIR "/src/DeviceSimulator"
RUN dotnet build "DeviceSimulator.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "DeviceSimulator.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "DeviceSimulator.dll"]
