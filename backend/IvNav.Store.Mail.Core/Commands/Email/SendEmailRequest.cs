using Ardalis.GuardClauses;
using MediatR;

namespace IvNav.Store.Mail.Core.Commands.Email;

public class SendEmailRequest : IRequest<SendEmailResponse>
{
    public string To { get; }

    public string Subject { get; }

    public string Body { get; }

    public SendEmailRequest(string to, string subject, string body)
    {
        To = Guard.Against.NullOrEmpty(to);
        Subject = Guard.Against.NullOrEmpty(subject);
        Body = Guard.Against.NullOrEmpty(body);
    }
}
