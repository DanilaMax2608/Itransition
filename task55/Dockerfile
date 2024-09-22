# Используем официальный образ .NET
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app

# Стадия сборки
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Копируем файл проекта из подкаталога
COPY task55/task55.csproj ./task55/
WORKDIR /src/task55
RUN dotnet restore "./task55.csproj"

# Копируем остальные файлы
COPY task55/. ./

# Строим проект
RUN dotnet build "task55.csproj" -c Release -o /app/build

# Стадия публикации
FROM build AS publish
RUN dotnet publish "task55.csproj" -c Release -o /app/publish

# Стадия финального образа
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "task55.dll"]
