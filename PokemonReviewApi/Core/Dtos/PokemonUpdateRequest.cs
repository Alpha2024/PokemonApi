﻿namespace PokemonReviewApi.Core.Dtos
{
    public class PokemonUpdateRequest
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime BirthDate { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
