using DataAccessLayer.Data;
using DataAccessLayer.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DataAccessLayer.Interfaces
{
    public class DogParksRepository : IDogParksRepository
    {
        private readonly DogParksDBContext dogParksDBContext;

        public DogParksRepository(DogParksDBContext dogParksDBContext)
        {
            this.dogParksDBContext = dogParksDBContext;
        }

        public async Task<IEnumerable<DogParksModel>> GetDogParksAsync()
        {
            return await dogParksDBContext.DogParks.ToListAsync();
        }
    }
}