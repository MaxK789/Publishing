# Publishing

This project uses Entity Framework Core migrations to manage the SQL Server database schema.

## Applying migrations

1. Restore dependencies with `dotnet restore`.
2. Run migrations to create or update the database:
   ```bash
   dotnet ef database update --project src/Publishing.Infrastructure --startup-project src/Publishing.UI
   ```

The application automatically applies pending migrations on startup.
