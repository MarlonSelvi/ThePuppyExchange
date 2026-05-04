namespace ThePuppyExchange.Models
{
    public class SurveyViewModel
    {
        public string Lifestyle { get; set; }
        public string Household { get; set; }
        public int? HomebodyRating { get; set; }
        public string IncomeRange { get; set; }
    }

    public class SurveyResultViewModel
    {
        public string RecommendedBreed { get; set; }
        public string Reasoning { get; set; }
    }
}
