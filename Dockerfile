# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /app

# Copy csproj and restore dependencies
COPY *.csproj ./
RUN dotnet restore

# Copy the rest of the project and publish
COPY . ./
RUN dotnet publish -c Release -o out

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app

# Copy published files from build stage
COPY --from=build /app/out ./

# Expose port 10000 (you can change if needed)
EXPOSE 10000

# Run the app
ENTRYPOINT ["dotnet", "DisasterAlleviationFoundation.dll"]
