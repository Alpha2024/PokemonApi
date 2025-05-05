namespace PokemonReviewApi.Core.Dtos
{
    public class OwnerAddRequestDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
