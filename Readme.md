# Customer App

## Purpose

This is an example of DDD-CQRS-Clean based architecture.

## Run locally

- Install Docker Desktop
- Initialize secret locally:
```
 dotnet user-secrets set --project ./CustomerApp.RestApi/ "JwtSettings:Secret" "super-secret-key"
```

## Emails - Forgot password feature

To see sent emails, open the local SMTP server on http://localhost:3000

## To be implemented

- Update customer
- Real database running locally
- Manage concurrency (optimist update, auto retry)