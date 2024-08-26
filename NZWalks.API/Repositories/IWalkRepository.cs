using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public interface IWalkRepository
    {
        Task<IEnumerable<Walk>> GetAllAsync();
        Task<Walk> GetAsync(Guid Id);
        Task<Walk> AddAsync(Walk walk);
        Task<Walk> UpdateAsync(Guid Id, Walk walk);
        Task<Walk> DeleteAsync(Guid Id);
    }
}
