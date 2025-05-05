using Microsoft.AspNetCore.Mvc;
using PokemonReviewApi.Core.Dtos;
using PokemonReviewApi.Core.Models;
using PokemonReviewApi.Core.Repositories.IRepositories;
using System.Net.Mime;

namespace PokemonReviewApi.Controllers
{
    [Route("api/[controller]")]
    [Produces(MediaTypeNames.Application.Json)]
    [Consumes(MediaTypeNames.Application.Json)]
    [ApiController]
    public class ReviewController : ControllerBase
    {
        private readonly IReviewRepository _reviewRepository;

        public ReviewController(IReviewRepository reviewRepository)
        {
            _reviewRepository = reviewRepository;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Review))]
        public async Task<IActionResult> GetReviews()
        {
            var reviews = await _reviewRepository.GetReviews();
            if (reviews == null || !reviews.Any())
            {
                return NotFound("No reviews found.");
            }
            return Ok(reviews);
        }

        [HttpGet]
        [Route("{reviewId:int}", Name = "GetReview")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReview(int reviewId)
        {
            // Validate the reviewId
            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound($"Review with ID {reviewId} not found.");
            }
            if (reviewId < 1)
            {
                return BadRequest("Invalid review ID.");
            }
            var review = await _reviewRepository.GetReview(reviewId);

            return Ok(review);
        }
        [HttpGet("pokemon/{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Review))]
        [ProducesResponseType(400)]

        public async Task<IActionResult> GetReviewsOfAPokemon(int pokeId)
        {
            if (pokeId < 1)
            {
                return BadRequest("Invalid Pokemon ID.");
            }
            var reviews = await _reviewRepository.GetReviewsOfAPokemon(pokeId);
            if (reviews == null || !reviews.Any())
            {
                return NotFound($"No reviews found for Pokemon with ID {pokeId}.");
            }
            return Ok(reviews);
        }


        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReview([FromBody] ReviewAddRequestDto reviewAddRequet)
        {
            if (reviewAddRequet == null)
            {
                return BadRequest(ModelState);
            }
            var review = _reviewRepository.GetReviews().Result.FirstOrDefault(r => r.Title.Trim().ToUpper() == reviewAddRequet.Title.TrimEnd().ToUpper());

            if (review != null)
            {
                ModelState.AddModelError("", "Review already Exit");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {

                return BadRequest(ModelState);
            }

            var newReview = new Review
            {
                Title = reviewAddRequet.Title,
                CreatedAt = DateTime.Now,
                Text = reviewAddRequet.Text,
                Rating = reviewAddRequet.Rating,
            };



            if (!await _reviewRepository.CreateReview(newReview))
            {
                ModelState.AddModelError("", "Error in adding the Review");
                return StatusCode(500, ModelState);
            }

            return Ok("Review Added Successfuly");
        }

        [HttpPut("updateReview/{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateReview(int reviewId, [FromBody] ReviewUpdateRequestDto reviewAddRequet)
        {
            if (reviewAddRequet == null || reviewId < 1)
            {
                return BadRequest(ModelState);
            }
            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound($"Review with ID {reviewId} not found.");
            }
            var review = await _reviewRepository.GetReview(reviewId);
            review.Title = reviewAddRequet.Title;
            review.Text = reviewAddRequet.Text;
            review.Rating = reviewAddRequet.Rating;
            review.UpdatedAt = DateTime.Now;
            if (!await _reviewRepository.UpdateReview(review))
            {
                ModelState.AddModelError("", "Error in updating the Review");
                return StatusCode(500, ModelState);
            }
            return Ok("Review Updated Successfuly");
        }

        [HttpDelete("{reviewId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteReview(int reviewId)
        {
            if (reviewId < 1)
            {
                return BadRequest("Invalid review ID.");
            }
            if (!await _reviewRepository.ReviewExist(reviewId))
            {
                return NotFound($"Review with ID {reviewId} not found.");
            }
            var review = await _reviewRepository.GetReview(reviewId);
            if (!await _reviewRepository.DeleteReview(review))
            {
                ModelState.AddModelError("", "Error in deleting the Review");
                return StatusCode(500, ModelState);
            }
            return Ok("Review Deleted Successfuly");
        }
    }
}
