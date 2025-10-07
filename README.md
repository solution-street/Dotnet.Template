# Dotnet.Template.Server

## Testing
Naming conventions for xUnit tests:
`BehaviorTested_State_ExpectedResult`

Taken inspiration from:
https://learn.microsoft.com/en-us/dotnet/core/testing/unit-testing-best-practices#naming-your-tests

## Prerequisites
- [Visual Studio](https://visualstudio.microsoft.com/) or any other C# development environment.

### Environment variables
"ASPNETCORE_ENVIRONMENT": "Development",

- If you want auth0:
"CLIENT_ORIGIN_URL": "https://localhost:4200,http://localhost:4200",
"BEARER_TOKEN_AUDIENCE": "yourauth0audience",
"BEARER_TOKEN_AUTHORITY": "https://dev-yourdomain.us.auth0.com/"

### User Secrets
Create a database and add your connection string to your user secrets.

If you followed the steps above you should have the following set:
secrets.json:
```
{
  "ConnectionStrings": {
    "DATABASE_LABEL": "Server=...;Database=...;Trusted_Connection=True;TrustServerCertificate=True;"
  }
}
```

## Setup
### Clone the Repsitory
   ```bash
   git clone https://github.com/solution-street/Dotnet.Template.git
  ```

### Starting the Application
- To start the application (with debugging), you can press `F5` or _Debug > Start With Debugging_.

## Project Structure
### API
This is the main entry point into the application. This application layer is responsible for:
- Application startup: registering middleware, 3rd party dependencies, dependency injection (DI), etc.
- Controller/endpoint definitions.
  - This includes basic application flow within the controller method.
- User authorization and authentication.
- Global error handling and logging.

### Services
It is intended for this layer to be broken up into logical services to perform business actions. Services may (and should) call other services. (I.E. a `UserService` may call an `EmailService` to send a welcome email to a new user).

This layer is responsible for:
- Business logic, which includes:
  - Defining business rules (i.e. do this, send an email, save this)
  - Defining business entities that are used in the API's request(s)/response(s).
- Interacting with the `DataLayer`
	- This layer will map DTOs to/from database models.
	- Modify database models for business purposes and then persist them back to the `DataLayer`.
- Configurations related to business logic like:
  - AutoMapper profiles.
  - Dependency injection mapping (but not registration).
  - Validation logic. It is preferable to perform data transfer object (DTO) validation in a single location, and the service layer ensures that this logic is always ran (opposed to keeping it in the controller). 

### DataLayer
The data layer should contain all data access logic and knowledge:
- Repository classes to interact with the database. This ensures our data access logic (Entity Framework queries) are reusable and isolated.
- Database (EF) migrations.
- Database (EF) models.
- Data seeding scripts.

### API.Tests
This test project will handle end-to-end (E2E) testing and tests should enter directly through the API. A few notes on E2E tests:
- Tests will verify authorization/authentication.
- Tests should mock out application edges (database, 3rd party APIs, mail services, etc).
- Tests should assert overall behavior. For example, we should verify the API's output AND assert in memory database changes.

### Tests
This project will handle service layer unit tests. Tests should verify a single unit that is not easily verifiable (or is out of scope) of E2E behavior.
- Examples:
  - We want to test various mapping logic.
  - We want to test complex business logic behavior and method calls.
  - We want to test validation.
  - We want to test object behavior, helper methods, or extension methods.

### Core
This project will contain core logic that is required throughout all layers of the application. Some examples of code that will live here:
- Extension methods, constant values, helpers, custom exceptions, general models, and so on.
