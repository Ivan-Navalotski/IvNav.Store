using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Identity.Web.Models;

/// <summary>
///  Register request
/// </summary>
public class RegisterRequestDto
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