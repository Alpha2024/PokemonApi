﻿using System.ComponentModel.DataAnnotations;

namespace PokemonReviewApi.Core.Models
{
    public class Reviewer
    {
        [Key]
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public ICollection<Review> Reviews { get; set; }

    }
}
