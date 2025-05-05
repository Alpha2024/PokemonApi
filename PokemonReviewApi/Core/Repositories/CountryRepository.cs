using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class CountryRepository : ICountryRepository
    {
        private readonly DataContext _Context;

        public CountryRepository(DataContext dataContext)
        {
            this._Context = dataContext;
        }
        public async Task<bool> CountryExists(int id)
        {
            return await _Context.Countries.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCountry(Country country)
        {
            _Context.Countries.Add(country);

            return await Save();
        }

        public async Task<bool> DeleteCountry(Country country)
        {
            _Context.Countries.Remove(country);
            return await Save();
        }

        public async Task<ICollection<Country>> GetCountries()
        {
            return await _Context.Countries.ToListAsync();
        }

        public async Task<Country> GetCountry(int id)
        {
            return await _Context.Countries.FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Country> GetCountryByOwner(int ownerId)
        {
            return await _Context.Owners.Where(c => c.Id == ownerId)
                 .Select(c => c.Country).FirstOrDefaultAsync();

        }

        public async Task<ICollection<Owner>> GetOwnersFromACountry(int countryId)
        {
            return await _Context.Owners.Where(c => c.Country.Id == countryId).ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _Context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCountry(Country country)
        {
            _Context.Countries.Update(country);
            return await Save();
        }
    }
}
