FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
ARG RUNTIME=linux-x64
WORKDIR /src
COPY ["EinsTools.ServiceMonitor/EinsTools.ServiceMonitor.csproj", "EinsTools.ServiceMonitor/"]
RUN dotnet restore "EinsTools.ServiceMonitor/EinsTools.ServiceMonitor.csproj"
COPY . .
WORKDIR "/src/EinsTools.ServiceMonitor"
RUN dotnet build "EinsTools.ServiceMonitor.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "EinsTools.ServiceMonitor.csproj" -c $BUILD_CONFIGURATION -o /app/publish/$RUNTIME --self-contained true -r $RUNTIME


FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish/$RUNTIME .
WORKDIR /app
ENTRYPOINT ["dotnet", "EinsTools.ServiceMonitor.dll"]
