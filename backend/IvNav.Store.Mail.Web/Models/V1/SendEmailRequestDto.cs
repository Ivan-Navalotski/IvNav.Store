using System.ComponentModel.DataAnnotations;

namespace IvNav.Store.Mail.Web.Models.V1;

public class SendEmailRequestDto
{
    [Required]
    public string? To { get; init; }

    [Required]
    public string? Subject { get; init; }

    [Required]
    public string? Body { get; init; }
}
