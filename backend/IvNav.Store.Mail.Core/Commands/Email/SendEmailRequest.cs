using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Mail.Core.Commands.Email;

public class SendEmailRequest : IRequest<SendEmailResponse>
{
    public string Email { get; }

    public string Title { get; }

    public string Body { get; }

    public SendEmailRequest(string email, string title, string body)
    {
        Email = Guard.Against.NullOrEmpty(email);
        Title = Guard.Against.NullOrEmpty(title);
        Body = Guard.Against.NullOrEmpty(body);
    }
}
