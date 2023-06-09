using Ardalis.GuardClauses;
using MailKit.Net.Smtp;
using MediatR;
using Microsoft.Extensions.Configuration;
using MimeKit.Text;
using MimeKit;
using MailKit.Security;

namespace IvNav.Store.Mail.Core.Commands.Email;

internal class SendEmailCommand : IRequestHandler<SendEmailRequest, SendEmailResponse>
{
    private readonly IConfiguration _configuration;

    public SendEmailCommand(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        var emailMessage = new MimeMessage();

        var section = Guard.Against.Null(_configuration.GetSection("EmailConfiguration"));

        var settings = new
        {
            From = Guard.Against.Null(section.GetValue<string>("From")),
            SmtpServer = Guard.Against.Null(section.GetValue<string>("SmtpServer")),
            Port = Guard.Against.Null(section.GetValue<int>("SmtpServer")),
            UserName = Guard.Against.Null(section.GetValue<string>("UserName")),
            Password = Guard.Against.Null(section.GetValue<string>("Password")),
        };

        emailMessage.From.Add(MailboxAddress.Parse(settings.From));
        emailMessage.To.Add(MailboxAddress.Parse(request.To));
        emailMessage.Subject = request.Subject;
        emailMessage.Body = new TextPart(TextFormat.Html) { Text = request.Body };

        using var smtp = new SmtpClient();


        await smtp.ConnectAsync(settings.SmtpServer, settings.Port, SecureSocketOptions.StartTls, cancellationToken);
        await smtp.AuthenticateAsync(settings.UserName, settings.Password, cancellationToken);
        await smtp.SendAsync(emailMessage, cancellationToken);
        await smtp.DisconnectAsync(true, cancellationToken);

        return new SendEmailResponse();
    }
}
