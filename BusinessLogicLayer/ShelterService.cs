using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogicLayer
{
    public class ShelterService : IShelterService
    {
        private readonly IShelterRepository shelterRepository;

        public ShelterService(IShelterRepository shelterRepository)
        {
            this.shelterRepository = shelterRepository;
        }

        public async Task<IEnumerable<ShelterModel>> GetShelterModelsAsync()
        {
            return await shelterRepository.GetShelterModelsAsync();
        }
    }
}
