# Deepin Server
 
## Database Migrations
- Postgres
	- `dotnet ef migrations add InitialCreate -c IdentityContext -p ../Deepin.Infrastructure -o Migrations/Identity` 
	- `dotnet ef migrations add InitialCreate -c ConfigurationContext -p ../Deepin.Infrastructure -o Migrations/Configuration` 
	- `dotnet ef migrations add InitialCreate -c PersistedGrantContext -p ../Deepin.Infrastructure -o Migrations/PersistedGrant` 
	- `dotnet ef migrations add InitialCreate -c ChatDbContext -p ../Deepin.Infrastructure -o Migrations/Chat` 