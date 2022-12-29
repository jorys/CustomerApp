# Customer App

## Purpose

This is an example of DDD-CQRS-Clean based architecture.

<img src="./docs/CleanArchitecture.jpg" alt="drawing" width="300"/>

## How To

### How To - Run locally

- Install Docker Desktop
- Open ./CustomerApp.sln with Visual Studio
- Set "docker-compose" as startup project
- Press F5 to run Solution, a browser with Swagger will open

### How To - Call Customer routes

You must be authenticated to access these routes:
1. Call "register" or "login" to retrieve the customer JWT token
2. Put your token in Authorization header prefixed by "Bearer ". On swagger interface, you can use the Authorize button to do so.

### How To - Open MongoDb admin interface

Local mongo-express is accessible through http://localhost:8081

### How To - Check email

Local SMTP server is accessible through http://localhost:3000

## Next features to be implemented

- Manage concurrency (optimist update, auto retry)
- Add Unit and Integration Tests
- External API call with circuit breaker example
- Event-bus listener API
