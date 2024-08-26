using Microsoft.EntityFrameworkCore;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;

namespace NZWalks.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly NZWalksDBContext nZWalksDBContext;
        public RegionRepository(NZWalksDBContext nZWalksDBContext)
        {
            this.nZWalksDBContext = nZWalksDBContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await nZWalksDBContext.AddAsync(region);
            await nZWalksDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid Id)
        {
            var region = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
            if (region == null)
            {
                return region;
            }
            //Deleting the region
            nZWalksDBContext.Regions.Remove(region);
            await nZWalksDBContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await nZWalksDBContext.Regions.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid Id)
        {
            return await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);
        }

        public async Task<Region> UpdateAsync(Guid Id, Region region)
        {
            var existingRegion = await nZWalksDBContext.Regions.FirstOrDefaultAsync(x => x.Id == Id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Lat = region.Lat;
            existingRegion.Area = region.Area;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

            await nZWalksDBContext.SaveChangesAsync();
            return existingRegion;
        }
    }
}
