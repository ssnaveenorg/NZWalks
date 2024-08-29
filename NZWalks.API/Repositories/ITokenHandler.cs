using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface ITokenHandler
    {
        Task<string> CreateTokerAsync(User user);
    }
}
