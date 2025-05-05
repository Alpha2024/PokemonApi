using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class PokemonRepositories : IPokemonReposiotries
    {
        private readonly DataContext _context;
        public PokemonRepositories(DataContext dataContext)
        {
            _context = dataContext;
        }
        public async Task<ICollection<Pokemon>> GetPokemons()
        {
            return await _context.Pokemon.OrderBy(p => p.Id).ToListAsync();
        }

        public async Task<Pokemon> GetPokemon(int id)
        {
            return await _context.Pokemon.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<Pokemon> GetPokemon(string name)
        {
            return await _context.Pokemon.FirstOrDefaultAsync(p => p.Name.ToLower() == name.ToLower());
        }

        public async Task<decimal> GetPokemonRating(int pokeId)
        {
            var reviews = await _context.Reviews
                .Where(r => r.Pokemon.Id == pokeId)
                .ToListAsync();

            if (reviews.Count <= 0)
            {
                return 0m;
            }

            var average = (decimal)reviews.Sum(r => r.Rating) / reviews.Count;
            return average;
        }


        public async Task<bool> PokemonExists(int pokeID)
        {
            return await _context.Pokemon.AnyAsync(p => p.Id == pokeID);
        }

        public async Task<bool> Save()
        {
            var saved = await _context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> CreatePokemon(Pokemon pokemon, int ownerId, int categoryId)
        {
            var pokemonOwnerEntity = await _context.Owners.FirstOrDefaultAsync(a => a.Id == ownerId);

            var category = await _context.Categories.FirstOrDefaultAsync(c => c.Id == categoryId);

            var pokemonOwner = new PokemonOwner
            {
                Owner = pokemonOwnerEntity,
                Pokemon = pokemon
            };

            _context.Add(pokemonOwner);

            var pokemonCategory = new PokemonCategory
            {
                Category = category,
                Pokemon = pokemon
            };

            _context.Add(pokemonCategory);
            _context.Add(pokemon);

            return await Save();

        }

        public async Task<bool> UpdatePokemon(Pokemon pokemon, int ownerId, int categoryId)
        {
            _context.Update(pokemon);

            return await Save();
        }

        public async Task<bool> DeletePokemon(Pokemon pokemon)
        {
            _context.Remove(pokemon);

            return await Save();
        }
    }
}
