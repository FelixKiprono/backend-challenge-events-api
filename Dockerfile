# Use the official ASP.NET Core runtime image as the base image
FROM mcr.microsoft.com/dotnet/aspnet:6.0

# Set the working directory to /app
WORKDIR /app

# Copy the published output of the ASP.NET API project to the working directory
COPY ./bin/Release/net6.0/publish .

COPY ./events.db /app/

# Install the SQLite package
RUN apt-get update && \
    apt-get install -y sqlite3 libsqlite3-dev && \
    rm -rf /var/lib/apt/lists/*

# Set the environment variable for the SQLite database path
ENV ConnectionStrings__DefaultConnection="Data Source=./events.db"

# Expose port 80 for the ASP.NET API
EXPOSE 80

# Start the ASP.NET API
ENTRYPOINT ["dotnet", "demoapp.dll"]