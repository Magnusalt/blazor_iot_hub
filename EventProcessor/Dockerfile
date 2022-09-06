FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["EventProcessor/EventProcessor.csproj", "EventProcessor/"]
RUN dotnet restore "EventProcessor/EventProcessor.csproj"
COPY . .
WORKDIR "/src/EventProcessor"
RUN dotnet build "EventProcessor.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "EventProcessor.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "EventProcessor.dll"]
