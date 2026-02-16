using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataAccessLayer.Interfaces
{
    public class ShelterRepository : IShelterRepository
    {
        private readonly ShelterDBContext shelterDbContext;

        public ShelterRepository(ShelterDBContext shelterDbContext)
        {
            this.shelterDbContext = shelterDbContext;
        }

        public async Task<IEnumerable<ShelterModel>> GetShelterModelsAsync()
        {
            return await shelterDbContext.Shelter.ToListAsync();
        }
    }
}
