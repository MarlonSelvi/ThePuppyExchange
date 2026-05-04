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

        public Task<IEnumerable<PuppyModel>> GetPuppyModelsAsync()
            => puppyReposityory.GetPuppyModelsAsync();

        public Task<IEnumerable<string>> GetUniqueBreedsAsync()
            => puppyReposityory.GetUniqueBreedsAsync();
    }
}
