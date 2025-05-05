using Microsoft.EntityFrameworkCore;
using PokemonReviewApi.Core.Data;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;

namespace PokemonReviewApi.Core.Repositories
{
    public class ReviewerRepository : IReviewerRepository
    {
        private readonly DataContext _Context;

        public ReviewerRepository(DataContext dataContext)
        {
            _Context = dataContext;
        }

        public async Task<bool> CreateReviewer(Reviewer reviewer)
        {
            _Context.Add(reviewer);

            return await Save();

        }

        public async Task<bool> DeleteReviewer(Reviewer reviewer)
        {
            _Context.Reviewers.Remove(reviewer);
            return await Save();
        }

        public async Task<Reviewer> GetReviewer(int id)
        {
            return await _Context.Reviewers.FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<ICollection<Reviewer>> GetReviewers()
        {
            return await _Context.Reviewers.ToListAsync();
        }

        public async Task<ICollection<Review>> GetReviewsByReviewer(int reviewerId)
        {
            return await _Context.Reviews.Where(r => r.Reviewer.Id == reviewerId).ToListAsync();
        }

        public async Task<bool> ReviewerExists(int id)
        {
            return await _Context.Reviewers.AnyAsync(r => r.Id == id);
        }

        public async Task<bool> Save()
        {
            var saved = await _Context.SaveChangesAsync();

            return saved > 0 ? true : false;
        }

        public async Task<bool> UpdateReviewer(Reviewer reviewer)
        {
            _Context.Update(reviewer);

            return await Save();
        }
    }
}
