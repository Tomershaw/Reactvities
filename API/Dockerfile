# שלב 1 - בסיס להרצה
FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 80

# שלב 2 - בסיס לבנייה
FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src

# העתק את כל הפתרון
COPY . .

# תיכנס לספריית API
WORKDIR /src/API

# עכשיו Restore
RUN dotnet restore "API.csproj"

# עכשיו Publish
RUN dotnet publish "API.csproj" -c Release -o /app/publish

# שלב 3 - הרצה סופית
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]
