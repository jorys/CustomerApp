# Customer App

## Purpose

This is an example of DDD-CQRS-Clean based architecture.

<img src="./docs/CleanArchitecture.jpg" alt="drawing" width="300"/>

### Architecture

<img src="./docs/ArchitectureDetails.jpg" alt="drawing" width="500"/>

### Routes

<img src="./docs/AuthenticationRoutes.jpg" alt="drawing" width="500"/>

- **Register**: allows to create a customer and generate a bearer token. Password is saved hashed (not decryptable) and salted
- **Login**: check email and password, generates a bearer token
- **Forgot-password**: send an email with reset-password token (see how-to - check mail section)
- **Reset-password**: allows to update password with reset-password token

<img src="./docs/CustomerRoutes.jpg" alt="drawing" width="500"/>

- **Get** customer information
- **Patch customer**: only filled fields will be updated
- **Put password**: allows to update password
Delete customer

<img src="./docs/StockRoutes.jpg" alt="drawing" width="500"/>

- **Get** stocks
- **Add-Items**: to increase an item quantity
- **Remove-Items**: to decrease an item quantity

### Concurrent data access

- On register, second check on email unicity by repository
- On stock management, it is an optimistic update: thanks to saved version, we are able to know if resource has changed and needs a retry

## How To

### How To - Run in local

#### Prerequisite

Install [Docker Desktop](https://www.docker.com/products/docker-desktop/)

#### Run locally

In Visual Studio
- Open ./CustomerApp.sln with Visual Studio
- Set "docker-compose" as startup project
- Press F5 (or Ctrl+F5 without debugging), a browser will open on Swagger page

In VS Code:
- Press F5 (or Ctrl+F5 without debugging), a browser will open on Swagger page

### How To - Handle unauthorized response

You must be authenticated to access customer and stock routes:
1. Call "register" or "login" to retrieve the customer JWT token
2. Put your token in Authorization header prefixed by "Bearer ". On swagger interface, you can use the Authorize button to do so

### How To - Open MongoDb admin interface

Local mongo-express is accessible through http://localhost:8081

### How To - Check email

Local SMTP server is accessible through http://localhost:3000

### How To - Run tests

- Start app without debugging (Ctrl+F5)
- run command: 
```
dotnet test customerApp.sln
```

## Next features to be implemented

- Stock: manage idempotent calls (idempotency identifier)
- Add example of choreography (through events)
- External API call with circuit breaker example
- Add code first BDD changesets (with Liquibase)