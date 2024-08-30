using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;

        public UserRepository(NZWalksDBContext _nZWalksDBContext)
        {
            nZWalksDBContext = _nZWalksDBContext;
        }
        public async Task<User> Authenticate(string Username, string Password)
        {
            var user = await nZWalksDBContext.Users
                .FirstOrDefaultAsync(x => x.Username.ToLower() == Username.ToLower() && x.Password == Password);
            if (user == null)
            {
                return null;
            }
            var userRoles = await nZWalksDBContext.User_Roles.Where(x => x.UserId == user.Id).ToListAsync();
            if (userRoles.Any())
            {
                user.Roles = new List<string>();
                foreach (var role in userRoles)
                {
                    var UserRole = await nZWalksDBContext.Roles.FirstOrDefaultAsync(x => x.Id == role.Id);
                    if (UserRole != null)
                    {
                        user.Roles.Add(UserRole.Name);
                    }
                }
            }

            user.Password = null;
            return user;

        }
    }
}
