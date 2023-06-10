using Ardalis.GuardClauses;
using IvNav.Store.Identity.Core.Models.User;

namespace IvNav.Store.Identity.Core.Queries.User
{
    public class ReadUserResponse
    {
        public static ReadUserResponse NotExists = new();

        public UserModel? Item { get; }

        internal ReadUserResponse(UserModel item)
        {
            Item = Guard.Against.Null(item);
        }

        private ReadUserResponse()
        {

        }
    }
}
