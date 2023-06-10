using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Identity.Web.ViewModels;

public class SignInViewModel
{
    [Required]
    public string? Email { get; set; }

    [Required]
    public string? Password { get; set; }

    public bool RememberMe { get; set; }

    public string? ReturnUrl { get; set; }

    public bool IsValidReturnUrl { get; set; }

    public bool EnableLocalLogin { get; set; }

    public string? ClientName { get; set; }

    public bool IsNativeClient { get; set; }

}
