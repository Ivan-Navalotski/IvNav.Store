Add-Migration Init -Context AppDbContext
Add-Migration InitPersistedGranMigration -c PersistedGrantDbContext -o Migrations/IdentityServer/PersistedGrantDb
Add-Migration InitConfigurationMigration -c ConfigurationDbContext  -o Migrations/IdentityServer/ConfigurationDb

Update-Database -Context AppDbContext
Update-Database -Context PersistedGrantDbContext
Update-Database -Context ConfigurationDbContext
