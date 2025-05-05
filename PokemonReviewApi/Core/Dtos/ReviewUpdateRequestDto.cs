namespace PokemonReviewApi.Core.Dtos
{
    public class ReviewUpdateRequestDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
