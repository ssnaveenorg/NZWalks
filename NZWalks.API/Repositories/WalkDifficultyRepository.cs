using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly NZWalksDBContext _nZWalksDBContext;

        public WalkDifficultyRepository(NZWalksDBContext nZWalksDBContext)
        {
            this._nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await _nZWalksDBContext.WalkDifficulty.AddAsync(walkDifficulty);
            await _nZWalksDBContext.SaveChangesAsync();
            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid Id)
        {
            var existing = await _nZWalksDBContext.WalkDifficulty.FindAsync(Id);
            if (existing != null)
            {
                _nZWalksDBContext.Remove(existing);
                await _nZWalksDBContext.SaveChangesAsync();
            }
            return existing;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await _nZWalksDBContext.WalkDifficulty.ToListAsync();
        }

        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await _nZWalksDBContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid Id, WalkDifficulty walkDifficulty)
        {
            var walkDiff = await _nZWalksDBContext.WalkDifficulty.FindAsync(Id);
            if (walkDiff == null)
            {
                return null;
            }
            walkDiff.Code = walkDifficulty.Code;
            await _nZWalksDBContext.SaveChangesAsync();
            return walkDiff;
        }
    }
}
