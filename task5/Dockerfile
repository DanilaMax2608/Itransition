# Используем официальный образ .NET Core SDK
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ["task55/task55.csproj", "task5/"]
RUN dotnet restore "task55/task55.csproj"

# Копируем остальные файлы и публикуем проект
COPY . .
WORKDIR "/app/task55"
RUN dotnet publish "task55.csproj" -c Release -o out

# Используем официальный образ .NET Core ASP.NET для запуска приложения
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/task5/out ./
ENTRYPOINT ["dotnet", "task55.dll"]
