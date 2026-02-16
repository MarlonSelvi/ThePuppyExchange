using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using DataAccessLayer.Interfaces;

namespace BusinessLogicLayer
{
    public class PuppyService : IPuppyService
    {
        private readonly IPuppyRepository puppyReposityory;

        public PuppyService(IPuppyRepository puppyRepository)
        {
            this.puppyReposityory = puppyRepository;
        }

        public async Task<IEnumerable<PuppyModel>> GetPuppyModelsAsync()
        {
            return await puppyReposityory.GetPuppyModelsAsync();
        }
    }
}
