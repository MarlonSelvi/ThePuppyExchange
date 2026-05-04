using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class DogParksService : IDogParksService
    {
        private readonly IDogParksRepository dogParksRepository;

        public DogParksService(IDogParksRepository dogParksRepository)
        {
            this.dogParksRepository = dogParksRepository;
        }

        public async Task<IEnumerable<DogParksModel>> GetDogParksAsync()
        {
            return await dogParksRepository.GetDogParksAsync();
        }
    }
}
