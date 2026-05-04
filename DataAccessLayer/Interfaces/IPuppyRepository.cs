using DataAccessLayer.Entities;

namespace DataAccessLayer.Interfaces
{
    public interface IPuppyRepository
    {
        Task<IEnumerable<PuppyModel>> GetPuppyModelsAsync();
        Task<IEnumerable<string>> GetUniqueBreedsAsync();
    }
}
