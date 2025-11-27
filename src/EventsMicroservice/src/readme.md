
# How to: 

## Add migrations:
Run the command:

`repoRoot/src/EventsMicroservice/src> dotnet ef migrations add MigrationName --project "./Events.Infrastructure/" --startup-project "./Events.Api/" `

Migrations are applied automatically when the API is run. Alternatively run dotnet ef database update.

## Run database:

1. Use docker compose at the repo root
2. Run a postgres container using the following command: `docker run -d --name events-db -e POSTGRES_USER=postgres -e POSTGRES_PASSWORD=postgres -e POSTGRES_DB=eventsdb -p 5432:5432 postgres:latest`