using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Identity.Web.ViewModels;

public class SignInViewModel
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; init; }

    public bool IsLocalUrl { get; init; }

    public string? ClientName { get; init; }

    public bool EnableLocalLogin { get; init; }

    public string? LogoUri { get; init; }

}
