using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Mail.Web.Models.V1;

public class SendEmailRequestDto
{
    [Required]
    public string? Email { get; init; }

    [Required]
    public string? Title { get; init; }

    [Required]
    public string? Body { get; init; }
}
