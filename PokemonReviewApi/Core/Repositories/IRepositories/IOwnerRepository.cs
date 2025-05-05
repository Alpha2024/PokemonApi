using PokemonReviewApi.Core.Models;

namespace PokemonReviewApi.Core.Repositories.IRepositories
{
    public interface IOwnerRepository
    {

        Task<ICollection<Owner>> GetOwners();
        Task<Owner> GetOwner(int id);

        Task<ICollection<Owner>> GetOwnersOfAPokemon(int pokemonId);
        Task<ICollection<Pokemon>> GetPokemonsOfAnOwner(int ownerId);
        Task<bool> OwnerExists(int ownerId);

        Task<bool> CreateOwner(Owner owner);
        Task<bool> UpdateOwner(Owner owner);
        Task<bool> DeleteOwner(Owner owner);
        Task<bool> Save();


    }
}
