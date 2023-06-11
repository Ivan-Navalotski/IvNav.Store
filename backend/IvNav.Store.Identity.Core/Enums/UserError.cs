namespace IvNav.Store.Identity.Core.Enums;

internal static class UserErrors
{
    public enum ExternalIdError
    {
        InvalidExternalId,
    }

    public enum EmailError
    {
        InvalidEmail,
        UserNotExists,
        UserAlreadyExists,
        DuplicateEmail,
        EmailNotConfirmed,
        ConfirmationLinkSendingError,
        ErrorCreatingExternalLink,
    }

    public enum PasswordError
    {
        IncorrectPassword,
        PasswordTooShort,
        PasswordRequiresUniqueChars,
        PasswordRequiresNonAlphanumeric,
        PasswordRequiresLower,
        PasswordRequiresUpper,
    }

    public enum ReturnUrlError
    {
        InvalidReturnUrl,
    }
}
