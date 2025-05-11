# Use the .NET SDK image for building and running migrations
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
WORKDIR /src

# Copy the project files
COPY . ./

# Restore dependencies
RUN dotnet restore

# Publish the application
RUN dotnet publish "./Chatting-Service-Backend.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Use the ASP.NET Core runtime image for running the application
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS runtime
WORKDIR /app

# Copy the published application
COPY --from=build /app/publish .

# Expose the port the application runs on
EXPOSE 5062

# Set the entry point
ENTRYPOINT ["dotnet", "Chatting-Service-Backend.dll"]