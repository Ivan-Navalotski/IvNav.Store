using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Identity.Web.ViewModels.Account;

public class SignInViewModel
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; init; }
}
