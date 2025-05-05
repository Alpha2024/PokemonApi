using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class OwnerRepository : IOwnerRepository
    {
        private readonly DataContext _Context;

        public OwnerRepository(DataContext dataContext)
        {
            _Context = dataContext;
        }

        public async Task<bool> CreateOwner(Owner owner)
        {
            _Context.Owners.Add(owner);

            return await Save();
        }

        public async Task<bool> DeleteOwner(Owner owner)
        {
            _Context.Owners.Remove(owner);

            return await Save();
        }

        public async Task<Owner> GetOwner(int id)
        {
            return await _Context.Owners.FirstOrDefaultAsync(o => o.Id == id);
        }

        public async Task<ICollection<Owner>> GetOwners()
        {
            return await _Context.Owners.ToListAsync();
        }

        public async Task<ICollection<Owner>> GetOwnersOfAPokemon(int pokemonId)
        {
            return await _Context.PokemonOwners.Where(o => o.PokemonId == pokemonId).Select(o => o.Owner).ToListAsync();
        }

        public async Task<ICollection<Pokemon>> GetPokemonsOfAnOwner(int ownerId)
        {
            return await _Context.PokemonOwners.Where(o => o.OwnerId == ownerId).Select(p => p.Pokemon).ToListAsync();
        }

        public async Task<bool> OwnerExists(int ownerId)
        {
            return await _Context.Owners.AnyAsync(o => o.Id == ownerId);
        }

        public async Task<bool> Save()
        {
            var saved = await _Context.SaveChangesAsync();
            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateOwner(Owner owner)
        {
            _Context.Owners.Update(owner);

            return await Save();
        }
    }
}
