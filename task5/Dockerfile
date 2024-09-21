# Используем официальный образ .NET Core SDK
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY *.csproj ./
RUN dotnet restore

# Копируем остальные файлы и публикуем проект
COPY . ./
RUN dotnet publish -c Release -o out

# Используем официальный образ .NET Core ASP.NET для запуска приложения
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/out ./
ENTRYPOINT ["dotnet", "task55.dll"]
