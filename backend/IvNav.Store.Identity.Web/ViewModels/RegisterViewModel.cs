using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Identity.Web.ViewModels
{
    public class RegisterViewModel
    {
        /// <summary>
        /// Email
        /// </summary>
        /// <example>test@gmail.com</example>
        [Required]
        [MinLength(1)]
        public string? Email { get; init; }


        /// <summary>
        /// Password
        /// </summary>
        /// <example>1qaz!QAZ</example>
        [Required]
        [MinLength(1)]
        public string? Password { get; init; }

        /// <summary>
        /// Password
        /// </summary>
        /// <example>1qaz!QAZ</example>
        [Required]
        [MinLength(1)]
        public string? ConfirmPassword { get; init; }

        public string? ReturnUrl { get; set; }

        public bool IsNativeClient { get; set; }
    }
}
