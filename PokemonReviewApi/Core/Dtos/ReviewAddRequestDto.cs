namespace PokemonReviewApi.Core.Dtos
{
    public class ReviewAddRequestDto
    {
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
