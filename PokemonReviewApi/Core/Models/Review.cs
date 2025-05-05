using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApi.Core.Models
{
    public class Review
    {
        [Key]
        public int Id { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Reviewer Reviewer { get; set; }
        public Pokemon Pokemon { get; set; }

    }
}
