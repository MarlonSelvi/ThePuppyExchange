using System.Text;
using System.Text.Json;

namespace BusinessLogicLayer
{
    public class GeminiService : IGeminiService
    {
        private readonly HttpClient _http;
        private const string Endpoint =
            "https://generativelanguage.googleapis.com/v1beta/models/gemini-2.5-flash:generateContent";
        public GeminiService(HttpClient http)
        {
            _http = http;
        }

        public async Task<(string Breed, string Reasoning)> RecommendBreedAsync(
            string lifestyle,
            string household,
            int? homebodyRating,
            string incomeRange,
            IEnumerable<string> availableBreeds)
        {
            // Load Key from .env file
            var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");
            if (string.IsNullOrWhiteSpace(apiKey))
                throw new InvalidOperationException("GEMINI_API_KEY is not set.");

            var breedList = string.Join(", ", availableBreeds.Where(b => !string.IsNullOrWhiteSpace(b)));

            // Prompt: choose a breed from the list that best fits the user's lifestyle, household, homebody rating, and income range.
            var prompt = $@"
                You are a dog breed matchmaker. Choose ONE breed from the list below that best fits the user.
                You MUST pick a breed that appears exactly in the list. Do not invent breeds.

                Available breeds: {breedList}

                User answers:
                - Lifestyle (day to day): {lifestyle}
                - Household: {household}
                - Homebody rating (1=Strongly Agree, 5=Strongly Disagree): {homebodyRating}
                - Income range: {incomeRange}

                Respond ONLY with valid JSON in this exact shape:
                {{ ""breed"": ""<one breed from list>"", ""reasoning"": ""<2-3 sentence explanation>"" }}";

            var payload = new
            {
                contents = new[]
                {
                    new { parts = new[] { new { text = prompt } } }
                },
                // Temperature 0.7 for some creativity
                generationConfig = new { temperature = 0.7, responseMimeType = "application/json" }
            };

            var url = $"{Endpoint}?key={apiKey}";
            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

            var response = await _http.PostAsync(url, content);

            // Error handling
            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine("=== GEMINI ERROR ===");
                Console.WriteLine(errorBody);
                Console.WriteLine("====================");
                return ("Unavailable", $"Service unavailable ({(int)response.StatusCode}). Try again in a moment.");
            }
            var raw = await response.Content.ReadAsStringAsync();

            try
            {
                //Parse the response to extract the breed and reasoning
                using var doc = JsonDocument.Parse(raw);
                var text = doc.RootElement
                    .GetProperty("candidates")[0]
                    .GetProperty("content")
                    .GetProperty("parts")[0]
                    .GetProperty("text")
                    .GetString();

                using var inner = JsonDocument.Parse(text);
                var breed = inner.RootElement.GetProperty("breed").GetString();
                var reasoning = inner.RootElement.GetProperty("reasoning").GetString();

                return (breed, reasoning);
            }
            catch
            {
                return (
                    "Unavailable",
                    "We couldn't generate a recommendation right now. Please try again."
                );
            }
        }
    }
}