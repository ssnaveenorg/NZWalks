
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class StaticUserRepository : IUserRepository
    {
        private List<User> Users = new List<User>()
        {
            //new User()
            //{
            //    FirstName = "Read Only",
            //    LastName = "User",
            //    Email = "readonly@user.com",
            //    Id=Guid.NewGuid(),
            //    Username = "readonly@user.com",
            //    Password = "Readonly@user",
            //    Roles = new List<string>{"reader"}
            //}
        };
        public async Task<User> Authenticate(string Username, string Password)
        {
            var user = Users.Find(x => x.Username.Equals(Username, StringComparison.InvariantCultureIgnoreCase) && x.Password == Password);
            return user;
        }
    }
}
