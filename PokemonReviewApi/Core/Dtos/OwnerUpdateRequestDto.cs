namespace PokemonReviewApi.Core.Dtos
{
    public class OwnerUpdateRequestDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
