using Ardalis.GuardClauses;

namespace IvNav.Store.Identity.Core.Models.User;

public class ClientInfoModel
{
    public string ClientName { get; }

    public bool EnableLocalLogin { get; }

    public string? LogoUri { get; }

    public ClientInfoModel(string clientName, bool enableLocalLogin, string? logoUri)
    {
        ClientName = Guard.Against.NullOrEmpty(clientName);
        EnableLocalLogin = enableLocalLogin;
        LogoUri = logoUri;
    }
}
