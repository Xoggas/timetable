FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Timetable.Api/Timetable.Api.csproj", "Timetable.Api/"]
RUN dotnet restore "Timetable.Api/Timetable.Api.csproj"
COPY . .
WORKDIR "/src/Timetable.Api"
RUN dotnet build "Timetable.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Timetable.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
USER root
RUN mkdir -p /app/SoundFiles && chmod -R 777 /app/SoundFiles
USER $APP_UID
ENTRYPOINT ["dotnet", "Timetable.Api.dll"]
