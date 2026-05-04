namespace BusinessLogicLayer
{
    public interface IGeminiService
    {
        Task<(string Breed, string Reasoning)> RecommendBreedAsync(
            string lifestyle,
            string household,
            int? homebodyRating,
            string incomeRange,
            IEnumerable<string> availableBreeds);
    }
}