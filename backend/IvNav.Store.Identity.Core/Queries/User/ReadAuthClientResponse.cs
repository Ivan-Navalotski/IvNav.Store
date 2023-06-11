using IvNav.Store.Identity.Core.Models.User;

namespace IvNav.Store.Identity.Core.Queries.User;

public class ReadAuthClientResponse
{
    public ClientInfoModel? Result { get; }

    public ReadAuthClientResponse(ClientInfoModel? result)
    {
        Result = result;
    }
}
