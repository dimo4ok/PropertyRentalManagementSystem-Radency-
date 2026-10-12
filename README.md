# PropertyRental API

A RESTful API for managing rental properties, units, and rental applications. The application supports two user roles — **Applicant** and **Property Manager** — with role-based access control and JWT authentication.

## Features

- User registration and sign-in using ASP.NET Core Identity
- JWT bearer authentication
- Role-based authorization for Applicants and Property Managers
- Property and rental unit management
- Rental application creation, updating, submission, withdrawal, review, approval, and denial
- Pagination for collection endpoints
- Optimistic concurrency control using row-version tokens
- Entity Framework Core migrations and database seeding
- Centralized exception handling and structured request logging
- Swagger/OpenAPI documentation in the Development environment
- Unit tests for selected application handlers

## Tech Stack

- **.NET 10**
- ASP.NET Core Minimal APIs
- Entity Framework Core
- Microsoft SQL Server
- ASP.NET Core Identity
- JWT Bearer Authentication
- Custom Mediator implementation
- Result pattern for application operation outcomes
- FluentValidation and endpoint validation filters
- Serilog
- Swagger/OpenAPI
- xUnit, NSubstitute, and Bogus for testing

## Architecture

The solution follows Clean Architecture principles and separates HTTP handling, application use cases, domain logic, and infrastructure concerns.

```text
PropertyRental/
├── src/
│   ├── PropertyRental.API/
│   ├── PropertyRental.Application/
│   ├── PropertyRental.Domain/
│   └── PropertyRental.Infrastructure/
├── tests/
│   └── PropertyRental.Tests/
├── PropertyRental.sln
└── docker-compose.yml
```

### Projects

| Project                         | Responsibility                                                                                                                        |
| ------------------------------- | ------------------------------------------------------------------------------------------------------------------------------------- |
| `PropertyRental.API`            | Minimal API endpoints, authentication and authorization configuration, HTTP response mapping, exception handling, and request logging |
| `PropertyRental.Application`    | Application use cases, commands, queries, handlers, DTOs, validation, and application abstractions                                    |
| `PropertyRental.Domain`         | Domain entities, enums, and business rules                                                                                            |
| `PropertyRental.Infrastructure` | SQL Server and EF Core configuration, Identity persistence, repositories, query services, JWT services, and database seeding          |
| `PropertyRental.Tests`          | Automated tests for selected application handlers                                                                                     |

The application layer uses a custom Mediator implementation to dispatch commands and queries to their handlers. Handlers return `Result` objects, which are mapped to HTTP responses by the API layer.

Infrastructure dependencies are registered through dependency injection.

## Domain Overview

The main domain entities include:

- **User** — an application user managed through ASP.NET Core Identity.
- **Property** — a rental property managed by a Property Manager.
- **Unit** — a rentable unit associated with a property.
- **UnitType** — a lookup entity describing a unit type.
- **RentalApplication** — an applicant's request to rent a unit.
- **Residence** — residence history associated with a rental application.
- **ApplicationStatusHistory** — tracks rental application status changes.
- **Lease** — represents a lease associated with the rental domain.

The application defines separate roles for Applicants and Property Managers. Protected endpoints use role-based authorization policies.

## Getting Started

### Prerequisites

Install the following:

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) or another Docker environment
- Git

### 1. Clone the repository

```bash
git clone https://github.com/dimo4ok/PropertyRentalManagementSystem-Radency-.git
cd PropertyRental
```

### 2. Start SQL Server

The project includes a Docker Compose configuration for Microsoft SQL Server 2022.

```bash
docker compose up -d
```

Ensure SQL Server is accessible on port `1433` and that the credentials match the connection string configured for the API.

**Security note:** Do not use a real database password in a committed Compose file. Configure the SQL Server password through an environment variable or another local secret-management mechanism.

### 3. Configure application settings

Configure the database connection string and JWT options in `src/PropertyRental.API/appsettings.json`, `appsettings.Development.json`, or through environment variables.

Example configuration:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost,1433;Database=PropertyRentalDb;User Id=sa;Password=<your-password>;Encrypt=True;TrustServerCertificate=True"
  },
  "JwtOptions": {
    "Issuer": "PropertyRental",
    "Audience": "PropertyRental",
    "SecretKey": "<your-long-random-secret-key>",
    "ExpirationSeconds": 3600
  }
}
```

The values above are examples. Use a strong, private signing key and appropriate credentials for your environment. Do not commit production secrets.

The JWT configuration contains the following settings:

| Setting             | Description                          |
| ------------------- | ------------------------------------ |
| `Issuer`            | Expected token issuer                |
| `Audience`          | Expected token audience              |
| `SecretKey`         | Key used to sign and validate JWTs   |
| `ExpirationSeconds` | Configured token lifetime in seconds |

The application validates the JWT issuer, audience, lifetime, and signing key.

For PowerShell, environment variables can override the corresponding configuration values:

```powershell
$env:ConnectionStrings__DefaultConnection = "Server=localhost,1433;Database=PropertyRentalDb;User Id=sa;Password=<your-password>;Encrypt=True;TrustServerCertificate=True"
$env:JwtOptions__Issuer = "PropertyRental"
$env:JwtOptions__Audience = "PropertyRental"
$env:JwtOptions__SecretKey = "<your-long-random-secret-key>"
$env:JwtOptions__ExpirationSeconds = "3600"
```

### 4. Restore dependencies

Run from the repository root:

```bash
dotnet restore PropertyRental.sln
```

### 5. Build the solution

```bash
dotnet build PropertyRental.sln
```

### 6. Run the API

```bash
dotnet run --project src/PropertyRental.API/PropertyRental.API.csproj
```

At startup, the API applies pending EF Core migrations and runs the database seeder.

Swagger and Swagger UI are enabled in the Development environment. Once the application is running, open the URL printed in the console and navigate to `/swagger`.

### 7. Run the tests

```bash
dotnet test tests/PropertyRental.Tests/PropertyRental.Tests.csproj
```

The test project contains tests for selected application handlers. The existence of these tests does not imply complete test coverage of every endpoint or business scenario.

## Authentication and Authorization

The API supports two roles:

| Role              | Responsibility                                              |
| ----------------- | ----------------------------------------------------------- |
| `Applicant`       | Browse available units and manage rental applications       |
| `PropertyManager` | Manage properties and units, and review rental applications |

Authentication uses ASP.NET Core Identity and JWT bearer tokens.

### Password Requirements

The configured Identity password policy requires:

- At least 8 characters
- At least one digit
- At least one uppercase letter
- At least one lowercase letter
- At least one non-alphanumeric character

Email addresses must be unique. Email confirmation is not required by the current Identity configuration.

### Register an Account

`POST /api/auth/sign-up`

Example request:

```json
{
  "userName": "john.doe",
  "email": "john@example.com",
  "firstName": "John",
  "lastName": "Doe",
  "password": "ExamplePassword123!",
  "phoneNumber": "+40712345678",
  "userRole": "Applicant"
}
```

The `userRole` value is illustrative. The accepted JSON representation depends on the application's enum serialization configuration.

### Sign In

`POST /api/auth/sign-in`

Example request:

```json
{
  "userName": "john.doe",
  "password": "ExamplePassword123!"
}
```

The successful response returns an access token and user information. An illustrative response shape is:

```json
{
  "accessToken": "<jwt-access-token>",
  "expires": "2026-10-13T12:00:00+00:00",
  "userId": "00000000-0000-0000-0000-000000000001",
  "userName": "john.doe",
  "role": "Applicant"
}
```

For protected endpoints, provide the token using the HTTP `Authorization` header:

```http
Authorization: Bearer <jwt-access-token>
```

Use Swagger's authorization interface to test authenticated requests, if enabled in the generated API documentation.

## API Reference

All routes below are relative to the API base URL. Request and response examples use illustrative values. Collection endpoints support pagination where indicated.

### Authentication

| Method | Endpoint            | Description                          |
| ------ | ------------------- | ------------------------------------ |
| `POST` | `/api/auth/sign-up` | Register a user                      |
| `POST` | `/api/auth/sign-in` | Authenticate a user and obtain a JWT |

### Property Management

These endpoints are intended for Property Managers.

| Method   | Endpoint                       | Description                             |
| -------- | ------------------------------ | --------------------------------------- |
| `GET`    | `/api/manager/properties`      | Retrieve a paginated list of properties |
| `GET`    | `/api/manager/properties/{id}` | Retrieve a property by ID               |
| `POST`   | `/api/manager/properties`      | Create a property                       |
| `PUT`    | `/api/manager/properties/{id}` | Update a property                       |
| `DELETE` | `/api/manager/properties/{id}` | Delete a property                       |

Create property request:

```json
{
  "name": "Central Residence",
  "address": "10 Example Street, Bucharest"
}
```

Update property request:

```json
{
  "name": "Central Residence",
  "address": "12 Example Street, Bucharest",
  "rowVersion": "<row-version-token>"
}
```

Delete property request body:

```json
{
  "rowVersion": "<row-version-token>"
}
```

### Unit Management

Available units can be browsed by Applicants. Creating, updating, and deleting units are Property Manager operations.

| Method   | Endpoint                                     | Description                                  |
| -------- | -------------------------------------------- | -------------------------------------------- |
| `GET`    | `/api/applicant/units`                       | Retrieve a paginated list of available units |
| `POST`   | `/api/manager/properties/{propertyId}/units` | Create a unit under a property               |
| `PUT`    | `/api/manager/units/{id}`                    | Update a unit                                |
| `DELETE` | `/api/manager/units/{id}`                    | Delete a unit                                |

Create unit request:

```json
{
  "unitNumber": "A-12",
  "bedrooms": 2,
  "monthlyRent": 750.0,
  "unitTypeId": "00000000-0000-0000-0000-000000000002"
}
```

Update unit request:

```json
{
  "unitNumber": "A-12",
  "bedrooms": 2,
  "monthlyRent": 800.0,
  "unitTypeId": "00000000-0000-0000-0000-000000000002",
  "rowVersion": "<row-version-token>"
}
```

Delete unit request body:

```json
{
  "rowVersion": "<row-version-token>"
}
```

### Applicant Rental Applications

| Method | Endpoint                                           | Description                                      |
| ------ | -------------------------------------------------- | ------------------------------------------------ |
| `GET`  | `/api/applicant/rental-applications`               | Retrieve a paginated list of rental applications |
| `GET`  | `/api/applicant/rental-applications/{id}`          | Retrieve an application by ID                    |
| `POST` | `/api/applicant/rental-applications`               | Create a rental application                      |
| `PUT`  | `/api/applicant/rental-applications/{id}`          | Update a rental application                      |
| `POST` | `/api/applicant/rental-applications/{id}/submit`   | Submit an application                            |
| `POST` | `/api/applicant/rental-applications/{id}/withdraw` | Withdraw an application                          |

Create rental application request:

```json
{
  "unitId": "00000000-0000-0000-0000-000000000003",
  "fullName": "John Doe",
  "phone": "+40712345678",
  "email": "john@example.com",
  "currentAddress": "10 Example Street, Bucharest",
  "residenceHistory": []
}
```

The `residenceHistory` array can contain residence-history entries according to the application's request model.

### Property Manager Rental Application Review

| Method | Endpoint                                        | Description                                      |
| ------ | ----------------------------------------------- | ------------------------------------------------ |
| `GET`  | `/api/manager/rental-applications`              | Retrieve a paginated list of rental applications |
| `GET`  | `/api/manager/rental-applications/{id}`         | Retrieve an application by ID                    |
| `POST` | `/api/manager/rental-applications/{id}/approve` | Approve an application                           |
| `POST` | `/api/manager/rental-applications/{id}/return`  | Return an application for further information    |
| `POST` | `/api/manager/rental-applications/{id}/deny`    | Deny an application                              |

Approve request body:

```json
{
  "rowVersion": "<row-version-token>"
}
```

Return or deny request body:

```json
{
  "comment": "Please provide additional information.",
  "rowVersion": "<row-version-token>"
}
```

The review request models include a row-version token to support concurrency checks.

## Pagination

Collection endpoints accept pagination parameters.

| Parameter    | Description              |
| ------------ | ------------------------ |
| `PageNumber` | Requested page number    |
| `PageSize`   | Number of items per page |

Example:

```http
GET /api/manager/properties?PageNumber=1&PageSize=10
```

Rental application listing endpoints also support optional filtering using the `Status` and `PropertyId` query parameters, where supported by the request model.

Paginated responses use a common structure:

```json
{
  "page": 1,
  "totalPages": 3,
  "pageSize": 10,
  "totalItems": 25,
  "items": []
}
```

The `items` array contains the DTOs appropriate to the requested endpoint.

## Optimistic Concurrency

Properties, units, and rental applications use row-version values for concurrency control.

Update and delete requests for properties and units include a `rowVersion` value. Rental application review operations also accept a row-version value.

When an EF Core concurrency conflict occurs, the global exception handler maps `DbUpdateConcurrencyException` to HTTP `409 Conflict`.

Use the row-version value returned by the API rather than inventing or reusing a placeholder token. The exact token representation depends on the API's DTO serialization.

## Error Handling

Application handlers return `Result` objects, which are converted into HTTP responses by the API layer.

Failed results are returned as JSON containing an `errors` field:

```json
{
  "errors": []
}
```

The contents of `errors` depend on the error returned by the application operation.

The global exception handler logs unhandled exceptions and maps them to HTTP responses:

- **409 Conflict** — EF Core concurrency conflicts
- **500 Internal Server Error** — other unhandled exceptions

Validation and application errors may use their own status codes through the Result-to-HTTP mapping.

## Database and Migrations

Entity Framework Core is configured to use Microsoft SQL Server.

The API automatically applies pending migrations and runs database seeding during startup.

The infrastructure project includes a design-time `AppDbContextFactory` for EF Core tooling. It loads configuration from the API project's `appsettings.json`, optionally loads `appsettings.Development.json`, and applies environment-variable overrides.

Ensure the database is running and the connection string is configured before using EF Core tooling.

The migration history includes changes for:

- Initial database schema
- Row-version concurrency support for properties and units
- Lease-to-unit relationship changes
- Row-version support for rental applications
- Indexing improvements

## Logging

Serilog is configured through application settings. The API also enables HTTP request logging through Serilog middleware.

Unhandled exceptions are logged by the global exception handler.

## Testing

Run the test suite with:

```bash
dotnet test tests/PropertyRental.Tests/PropertyRental.Tests.csproj
```

The existing tests cover selected application handlers, including authentication and property creation scenarios. Additional integration tests for HTTP endpoints, authorization, database behavior, and concurrency would further strengthen coverage.

## Future Improvements

Potential next steps include:

- Expand unit and integration test coverage
- Add automated API testing for authorization and validation
- Improve configuration validation for required JWT settings
- Add CI workflows for building and testing pull requests
- Document all DTO response schemas and domain status transitions
- Add deployment-specific configuration and health checks

## License

No license has been specified. Add a `LICENSE` file and update this section if the repository is intended for public reuse.
