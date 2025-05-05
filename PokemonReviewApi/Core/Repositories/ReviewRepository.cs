using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class ReviewRepository : IReviewRepository
    {
        private readonly DataContext _Context;

        public ReviewRepository(DataContext dataContext)
        {
            _Context = dataContext;
        }

        public async Task<bool> CreateReview(Review review)
        {
            _Context.Reviews.Add(review);

            return await Save();
        }

        public async Task<bool> DeleteReview(Review review)
        {
            _Context.Reviews.Remove(review);
            return await Save();
        }

        public async Task<bool> DeleteReviews(Review review)
        {
            _Context.RemoveRange(review);
            return await Save();
        }

        public async Task<Review> GetReview(int reviewId)
        {
            return await _Context.Reviews.FirstOrDefaultAsync(r => r.Id == reviewId);
        }

        public async Task<ICollection<Review>> GetReviews()
        {
            return await _Context.Reviews.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId)
        {
            return _Context.Reviews.Where(r => r.Pokemon.Id == pokeId).ToList();
        }

        public async Task<bool> ReviewExist(int reviewId)
        {
            return await _Context.Reviews.AnyAsync(r => r.Id == reviewId);
        }

        public async Task<bool> Save()
        {
            var saved = await _Context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReview(Review review)
        {
            _Context.Reviews.Update(review);
            return await Save();
        }
    }
}
