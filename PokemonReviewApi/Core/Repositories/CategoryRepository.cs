using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly DataContext _Context;

        public CategoryRepository(DataContext dataContext)
        {
            _Context = dataContext;
        }

        public async Task<bool> CategoryExists(int id)
        {
            return await _Context.Categories.AnyAsync(c => c.Id == id);
        }

        public async Task<bool> CreateCategory(Category category)
        {
            _Context.Categories.Add(category);
            return await Save();
        }

        public async Task<bool> DeleteCategory(Category category)
        {
            _Context.Categories.Remove(category);

            return await Save();
        }

        public async Task<ICollection<Category>> GetCategories()
        {
            return await _Context.Categories.ToListAsync();
        }

        public async Task<Category> GetCategory(int id)
        {
            return await _Context.Categories.FirstOrDefaultAsync(temp => temp.Id == id);
        }

        public async Task<ICollection<Pokemon>> GetPokemonByCategory(int categoryId)
        {
            return await _Context.PokemonCategories
                .Where(pc => pc.CategoryId == categoryId)
                .Select(p => p.Pokemon)
                .ToListAsync();
        }

        public async Task<bool> Save()
        {
            var saved = await _Context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateCategory(Category category)
        {
            _Context?.Categories.Update(category);
            return await Save();
        }
    }
}
