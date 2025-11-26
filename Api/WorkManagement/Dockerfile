# =========================
# 1. Build stage
# =========================
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

WORKDIR /src

# Copy solution file
COPY WorkManagement.sln .

# Copy từng project csproj
COPY Api/WorkManagement/WorkManagement.csproj Api/WorkManagement/
COPY Api/Work.DataContext/Work.DataContext.csproj Api/Work.DataContext/

# Restore packages
RUN dotnet restore WorkManagement.sln

# Copy toàn bộ source code
COPY . .

# Publish API
RUN dotnet publish Api/WorkManagement/WorkManagement.csproj -c Release -o /app/publish


# =========================
# 2. Runtime stage
# =========================
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS final

WORKDIR /app
COPY --from=build /app/publish .

EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

ENTRYPOINT ["dotnet", "WorkManagement.dll"]
