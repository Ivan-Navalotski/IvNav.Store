using Ardalis.GuardClauses;
using IvNav.Store.Core.Models.User;

namespace IvNav.Store.Core.Queries.User
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
