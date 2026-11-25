# Multi-stage Dockerfile for Employee Management System (.NET 10 Web API)

# Stage 1: Build & Publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

# Copy solution and csproj files first for efficient caching
COPY ["Domain/Domain.csproj", "Domain/"]
COPY ["Application/Application.csproj", "Application/"]
COPY ["Infrastructure/Infrastructure.csproj", "Infrastructure/"]
COPY ["Host/Host.csproj", "Host/"]
COPY ["Tests/EmployeeSystem.Tests/EmployeeSystem.Tests.csproj", "Tests/EmployeeSystem.Tests/"]

# Restore packages
RUN dotnet restore "Host/Host.csproj"

# Copy full source tree
COPY . .

# Build and Publish Host project
WORKDIR "/src/Host"
RUN dotnet publish "Host.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS runtime
WORKDIR /app

# Configure ASP.NET Core environment
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
EXPOSE 8080

# Copy published binaries from build stage
COPY --from=build /app/publish .

# Set entrypoint
ENTRYPOINT ["dotnet", "Host.dll"]
