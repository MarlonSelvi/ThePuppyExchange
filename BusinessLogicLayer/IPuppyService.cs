using DataAccessLayer.Entities;

namespace BusinessLogicLayer
{
    public interface IPuppyService
    {
        Task<IEnumerable<PuppyModel>> GetPuppyModelsAsync();
        Task<IEnumerable<string>> GetUniqueBreedsAsync();

    }
}
