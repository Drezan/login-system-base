# Login System Template

This is a basic login system template built with C# that provides a foundation for implementing user authentication and JWT-based authorization in your applications.

## Overview

This template includes a complete implementation of:
- User authentication (login/register)
- JWT (JSON Web Token) token generation and validation
- Database integration for user management
- Secure password handling
- Authorization middleware

## Getting Started

### Prerequisites
- .NET Core SDK
- SQL Database (configured in AppSettings)

### Configuration

Before running the application, you need to configure the `appsettings.json` file with:

1. **Database Connection**: Set your database connection string
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "your_database_connection_string"
   }
   ```

2. **JWT Settings**: Configure your JWT authentication parameters
   ```json
   "JwtSettings": {
     "SecretKey": "your_secret_key",
     "Issuer": "your_issuer",
     "Audience": "your_audience",
     "ExpirationMinutes": 60
   }
   ```

### Running the Application

1. Restore dependencies:
   ```bash
   dotnet restore
   ```

2. Update the database:
   ```bash
   dotnet ef database update
   ```

3. Run the application:
   ```bash
   dotnet run
   ```

The application will start and be ready to handle authentication requests.
