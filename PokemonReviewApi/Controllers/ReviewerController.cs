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
    public class ReviewerController : ControllerBase
    {
        private readonly IReviewerRepository _reviewerRepository;

        public ReviewerController(IReviewerRepository reviewerRepository)
        {
            _reviewerRepository = reviewerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReviewers()
        {
            var reviewers = await _reviewerRepository.GetReviewers();

            if (reviewers == null || reviewers.Count == 0)
            {
                return NotFound("No reviewers found.");
            }
            return Ok(reviewers);
        }

        [HttpGet("{reviewerId}")]
        [ProducesResponseType(200, Type = typeof(Reviewer))]
        [ProducesResponseType(404)]

        public async Task<IActionResult> GetReviewer(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound($"Reviewer with ID {reviewerId} not found.");
            }
            var reviewer = await _reviewerRepository.GetReviewer(reviewerId);

            return Ok(reviewer);
        }

        [HttpGet("{reviewerId}/reviews")]
        [ProducesResponseType(200, Type = typeof(ICollection<Review>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetReviewsByReviewer(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound($"Reviewer with ID {reviewerId} not found.");
            }
            var reviews = await _reviewerRepository.GetReviewsByReviewer(reviewerId);
            return Ok(reviews);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateReviewer([FromBody] ReviewerAddRequestDto reviewerAddRequest)
        {
            if (reviewerAddRequest == null)
            {
                return BadRequest(ModelState);
            }

            var reviwer = _reviewerRepository.GetReviewers().Result.FirstOrDefault(r => r.FirstName.Trim().ToUpper() == reviewerAddRequest.FirstName.Trim().ToUpper() && r.LastName.Trim().ToUpper() == reviewerAddRequest.LastName.Trim().ToUpper());

            if (reviwer != null)
            {
                ModelState.AddModelError("", "Reviewer already Exist");
                return StatusCode(422, ModelState);
            }

            var newReviewer = new Reviewer
            {
                FirstName = reviewerAddRequest.FirstName,
                LastName = reviewerAddRequest.LastName,
            };

            if (!await _reviewerRepository.CreateReviewer(newReviewer))
            {
                ModelState.AddModelError("", "Error in adding the Reviewer");
                return StatusCode(500, ModelState);
            }

            return Ok("Reviewer Added Successfully");
        }

        [HttpPut("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateReviewer(int reviewerId, [FromBody] ReviewerUpdateRequestDto reviewerUpdateRequest)
        {
            if (reviewerUpdateRequest == null)
            {
                return BadRequest(ModelState);
            }
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound($"Reviewer with ID {reviewerId} not found.");
            }
            var reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            reviewer.FirstName = reviewerUpdateRequest.FirstName;
            reviewer.LastName = reviewerUpdateRequest.LastName;
            if (!await _reviewerRepository.UpdateReviewer(reviewer))
            {
                ModelState.AddModelError("", "Error in updating the Reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Reviewer Updated Successfully");
        }

        [HttpDelete("{reviewerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteReviewer(int reviewerId)
        {
            if (!await _reviewerRepository.ReviewerExists(reviewerId))
            {
                return NotFound($"Reviewer with ID {reviewerId} not found.");
            }
            var reviewer = await _reviewerRepository.GetReviewer(reviewerId);
            var reviews = await _reviewerRepository.GetReviewsByReviewer(reviewerId);

            if (!await _reviewerRepository.DeleteReviewer(reviewer))
            {
                ModelState.AddModelError("", "Error in deleting the Reviewer");
                return StatusCode(500, ModelState);
            }
            return Ok("Reviewer Deleted Successfully");
        }
    }
}
