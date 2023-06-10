using IvNav.Store.Common.Identity;
using IvNav.Store.Infrastructure.Abstractions.Contexts;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Moq;
using NUnit.Framework;

namespace IvNav.Store.Testing.Helpers;

public class CoreTestFixture<TContext> : IDisposable where TContext : DbContext, IContextBase
{
    private bool _disposed;

    private readonly Guid _tenantId = Guid.Parse("7fc2daf2-cef0-4bab-89eb-718fb29297d2");
    private readonly Guid _userId = Guid.Parse("144ff3a4-2549-4708-9f74-e8273f85d138");

    public TContext AppDbContext { get; }
    public Mock<IMediator> MediatorMock { get; }

    public CoreTestFixture()
    {
        var options = new DbContextOptionsBuilder<TContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .ConfigureWarnings(builder => builder.Ignore(InMemoryEventId.TransactionIgnoredWarning))
            .Options;


        IdentityState.SetCurrent(_tenantId, _userId);

        var type = typeof(TContext);
        AppDbContext = (TContext)Activator.CreateInstance(type, options)!;
        AppDbContext.Database.EnsureCreated();

        MediatorMock = new(MockBehavior.Strict);
    }

    [SetUp]
    public virtual void BeforeEachTest()
    {
        MediatorMock.Invocations.Clear();
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
            AppDbContext.Dispose();
        }

        _disposed = true;
    }
}