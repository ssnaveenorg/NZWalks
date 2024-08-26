using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly NZWalksDBContext _nZWalksDBContext;

        public WalkRepository(NZWalksDBContext nZWalksDBContext)
        {
            this._nZWalksDBContext = nZWalksDBContext;
        }
        public async Task<Walk> AddAsync(Walk walk)
        {
            walk.Id = Guid.NewGuid();
            await _nZWalksDBContext.Walks.AddAsync(walk);
            await _nZWalksDBContext.SaveChangesAsync();
            return walk;
        }

        public async Task<Walk> DeleteAsync(Guid Id)
        {
            var walk = await _nZWalksDBContext.Walks.FindAsync(Id);
            if (walk == null)
            {
                return null;
            }
            _nZWalksDBContext.Walks.Remove(walk);
            await _nZWalksDBContext.SaveChangesAsync();
            return walk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
            return await
                _nZWalksDBContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid Id)
        {
            return
                _nZWalksDBContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Walk> UpdateAsync(Guid Id, Walk walk)
        {
            var existingWalk = await _nZWalksDBContext.Walks.FindAsync(Id);
            if (existingWalk == null)
            {
                return null;
            }
            existingWalk.Name = walk.Name;
            existingWalk.Length = walk.Length;
            existingWalk.RegionId = walk.RegionId;
            existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
            existingWalk.Id = Id;
            await _nZWalksDBContext.SaveChangesAsync();
            return existingWalk;
        }
    }
}
