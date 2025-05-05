using PokemonReviewApi.Core.Models;

namespace PokemonReviewApi.Core.Repositories.IRepositories
{
    public interface IPokemonReposiotries
    {
        Task<ICollection<Pokemon>> GetPokemons();
        Task<Pokemon> GetPokemon(int id);
        Task<Pokemon> GetPokemon(string name);
        Task<decimal> GetPokemonRating(int pokeId);

        Task<bool> PokemonExists(int pokeID);

        Task<bool> CreatePokemon(Pokemon pokemon, int ownerId, int categoryId);
        Task<bool> UpdatePokemon(Pokemon pokemon, int ownerId, int categoryId);
        Task<bool> DeletePokemon(Pokemon pokemon);
        Task<bool> Save();


    }
}
