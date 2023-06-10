namespace IvNav.Store.Identity.Web.Models.V1.Account;

/// <summary>
/// Login response
/// </summary>
public class LoginResponseDto
{
    /// <summary>
    /// Jwt token
    /// </summary>
    /// <example>eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxMjM0NTY3ODkwIiwibmFtZSI6IkpvaG4gRG9lIiwiaWF0IjoxNTE2MjM5MDIyfQ.SflKxwRJSMeKKF2QT4fwpMeJf36POk6yJV_adQssw5c</example>
    public string Token { get; init; } = null!;
}
