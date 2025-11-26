# ===== Runtime Image (.NET 9) =====
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
EXPOSE 5000

# Set timezone Asia/Ho_Chi_Minh
RUN ln -fs /usr/share/zoneinfo/Asia/Ho_Chi_Minh /etc/localtime \
    && dpkg-reconfigure -f noninteractive tzdata

ENV ASPNETCORE_ENVIRONMENT=Production
ENV ASPNETCORE_URLS=http://+:5000

# ===== Final Stage =====
FROM base AS final
WORKDIR /app

# Copy published output
COPY ./bin/Release/PublishOutput /app

# Copy jwt keys
COPY ./Jwtkeys /app/jwtkeys

ENTRYPOINT ["dotnet", "WorkManagement.dll"]
