using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Web.Models.V1.Account;

/// <summary>
/// Login request
/// </summary>
public class LoginRequestDto
{
    /// <summary>
    /// Email
    /// </summary>
    /// <example>test@gmail.com</example>
    [Required]
    public string? Email { get; init; }


    /// <summary>
    /// Password
    /// </summary>
    /// <example>1qaz!QAZ</example>
    [Required]
    public string? Password { get; init; }
}
