using MediatR;

namespace IvNav.Store.Mail.Core.Commands.Email;

internal class SendEmailCommand : IRequestHandler<SendEmailRequest, SendEmailResponse>
{
    public Task<SendEmailResponse> Handle(SendEmailRequest request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
