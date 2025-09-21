FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Timetable.Frontend/Timetable.Frontend.csproj", "Timetable.Frontend/"]
RUN dotnet restore "Timetable.Frontend/Timetable.Frontend.csproj"
COPY . .
WORKDIR "/src/Timetable.Frontend"
RUN dotnet build "Timetable.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "Timetable.Frontend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Timetable.Frontend.dll"]
