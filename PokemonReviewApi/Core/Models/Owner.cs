using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApi.Core.Models
{
    public class Owner
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Gym { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public Country Country { get; set; }

        public ICollection<PokemonOwner> PokemonOwners { get; set; }

    }
}
