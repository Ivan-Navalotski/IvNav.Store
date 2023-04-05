using IvNav.Store.Common.Identity;
using IvNav.Store.Infrastructure.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace IvNav.Store.Core.Tests.Helpers;

internal class DatabaseFixture : IDisposable
{
    private bool _disposed;

    private readonly Guid _tenantId = Guid.Parse("7fc2daf2-cef0-4bab-89eb-718fb29297d2");
    private readonly Guid _userId = Guid.Parse("144ff3a4-2549-4708-9f74-e8273f85d138");

    internal ApplicationDbContext ApplicationDbContext { get; }

    public DatabaseFixture()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;


        IdentityState.SetCurrent(_tenantId, _userId);

        ApplicationDbContext = new ApplicationDbContext(options);
        ApplicationDbContext.Database.EnsureCreated();
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
        {
            return;
        }

        if (disposing)
        {
            ApplicationDbContext.Dispose();
        }

        _disposed = true;
    }
}
