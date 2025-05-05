using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApi.Core.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public ICollection<PokemonCategory> PokemonCategories { get; set; }
    }
}
