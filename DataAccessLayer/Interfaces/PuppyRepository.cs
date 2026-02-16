using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

    }
}
