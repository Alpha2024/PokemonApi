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
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Category>))]
        public async Task<IActionResult> GetCategories()
        {

            var category = await _categoryRepository.GetCategories();

            return Ok(category);
        }

        [HttpGet("{categoryid}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCategoryById(int categoryid)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!await _categoryRepository.CategoryExists(categoryid))
                return NotFound();

            var category = await _categoryRepository.GetCategory(categoryid);
            if (category == null)
                return NotFound();
            var newCategory = new CategoryResponseDto
            {
                Name = category.Name,
                CreatedAt = category.CreatedAt
            };
            return Ok(newCategory);
        }


        [HttpGet("pokemon/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonByCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
                return NotFound();
            var pokemon = await _categoryRepository.GetPokemonByCategory(categoryId);
            if (pokemon == null)
                return NotFound();

            var pokemonList = pokemon.Select(p => new PokemonResponseDto
            {
                Name = p.Name,
                CreatedAt = p.CreatedAt
            }).ToList();

            return Ok(pokemonList);
        }

        [HttpPost("createCategory")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryAddRequestDto categorycreate)
        {
            if (categorycreate == null)
                return BadRequest(ModelState);

            var category = _categoryRepository.GetCategories().Result.Where(c => c.Name.Trim().ToUpper() == categorycreate.Name.Trim().ToUpper()).FirstOrDefault();

            if (category != null)
            {
                ModelState.AddModelError("", "Category already exists");
                return StatusCode(422, ModelState);
            }
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryMap = new Category
            {
                Name = categorycreate.Name,
                CreatedAt = DateTime.Now,
            };

            if (!await _categoryRepository.CreateCategory(categoryMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the category {categorycreate.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Successfully created category");
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdateCatgory(int categoryId, [FromBody] CategoryUpdateRequest categoryUpdateRequest)
        {
            if (categoryUpdateRequest == null)
            {
                return BadRequest(ModelState);
            }
            if (categoryId != categoryUpdateRequest.Id)
            {
                return BadRequest(ModelState);
            }

            if (!await _categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var updateCategory = new Category
            {
                Id = categoryUpdateRequest.Id,
                Name = categoryUpdateRequest.Name,
                UpdatedAt = DateTime.Now,
            };

            if (!await _categoryRepository.UpdateCategory(updateCategory))
            {
                ModelState.AddModelError("", "Error in updating the Category");
                return StatusCode(500, ModelState);
            }

            return NoContent();
        }

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory(int categoryId)
        {
            if (!await _categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }
            var category = await _categoryRepository.GetCategory(categoryId);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!await _categoryRepository.DeleteCategory(category))
            {
                ModelState.AddModelError("", "Error in deleting the Category");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
