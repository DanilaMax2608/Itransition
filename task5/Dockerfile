# Используем официальный образ .NET Core SDK
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /app

# Копируем csproj и восстанавливаем зависимости
COPY ["Itransition/task5/task55/task55.csproj", "Itransition/task5/task55/"]
WORKDIR "/app/Itransition/task5/task55"
RUN dotnet restore "task55.csproj"

# Копируем остальные файлы и публикуем проект
COPY . .
WORKDIR "/app/Itransition/task5/task55"
RUN dotnet publish "task55.csproj" -c Release -o out

# Используем официальный образ .NET Core ASP.NET для запуска приложения
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS runtime
WORKDIR /app
COPY --from=build /app/Itransition/task5/task55/out ./
ENTRYPOINT ["dotnet", "task55.dll"]
