using IvNav.Store.Infrastructure.Entities.Abstractions;

namespace IvNav.Store.Identity.Infrastructure.Entities;

public class UserExternalProviderLink : ITraceableEntity
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public User User { get; set; } = null!;

    public string Provider { get; set; } = null!;

    public string ExternalId { get; set; } = null!;
}
