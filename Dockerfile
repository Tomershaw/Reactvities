# שלב 1 - בסיס להרצה
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80

# שלב 2 - בסיס לבנייה
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# העתק את כל הקבצים
COPY . .

# שחזור התלויות
RUN dotnet restore "Reactivities.sln"

# בנייה ופרסום
RUN dotnet publish "API/API.csproj" -c Release -o /app/publish

# שלב 3 - בניית התמונה הסופית
FROM base AS final
WORKDIR /app
COPY --from=build-env /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
