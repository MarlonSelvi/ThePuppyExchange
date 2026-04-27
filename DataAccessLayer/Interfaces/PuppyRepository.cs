using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Interfaces
{
    public class PuppyRepository : IPuppyRepository
    {
        private readonly PuppyDbContext puppyDbContext;

        public PuppyRepository(PuppyDbContext puppyDbContext)
        {
            this.puppyDbContext = puppyDbContext;
        }

        public async Task<IEnumerable<PuppyModel>> GetPuppyModelsAsync()
        {
            return await puppyDbContext.Puppy.ToListAsync();
        }

        public async Task<IEnumerable<string>> GetUniqueBreedsAsync()
        {
            return await puppyDbContext.Puppy
                .Where(p => p.breed != null && p.breed != "")
                .Select(p => p.breed)
                .Distinct()
                .OrderBy(b => b)
                .ToListAsync();
        }
    }
}
