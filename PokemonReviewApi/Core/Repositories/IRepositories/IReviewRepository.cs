using PokemonReviewApi.Core.Models;

namespace PokemonReviewApi.Core.Repositories.IRepositories
{
    public interface IReviewRepository
    {
        Task<ICollection<Review>> GetReviews();
        Task<ICollection<Review>> GetReviewsOfAPokemon(int pokeId);
        Task<Review> GetReview(int reviewId);
        Task<bool> ReviewExist(int reviewId);
        Task<bool> CreateReview(Review review);
        Task<bool> UpdateReview(Review review);
        Task<bool> DeleteReview(Review review);
        Task<bool> DeleteReviews(Review review);
        Task<bool> Save();
    }
}
