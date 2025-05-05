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
    public class PokemonController : ControllerBase
    {
        private readonly IPokemonReposiotries _pokemonReposiotries;

        private readonly IReviewRepository _reviewRepository;

        public PokemonController(IPokemonReposiotries pokemonReposiotries, IReviewRepository reviewRepository)
        {
            _pokemonReposiotries = pokemonReposiotries;
            _reviewRepository = reviewRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(ICollection<Pokemon>))]
        public async Task<IActionResult> GetPokemons()
        {

            var pokemons = await _pokemonReposiotries.GetPokemons();
            if (pokemons == null)
                return NotFound();
            return Ok(pokemons);
        }

        [HttpGet("{pokeId}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        public async Task<IActionResult> GetPokemon(int pokeId)
        {
            if (!await _pokemonReposiotries.PokemonExists(pokeId))
                return NotFound();
            var pokemon = await _pokemonReposiotries.GetPokemon(pokeId);
            var newPokemon = new PokemonResponseDto
            {
                Name = pokemon.Name,
                BirthDate = pokemon.BirthDate,
                CreatedAt = pokemon.CreatedAt
            };

            return Ok(newPokemon);
        }

        [HttpGet("v2/{name}")]
        [ProducesResponseType(200, Type = typeof(Pokemon))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemon(string name)
        {
            if (name == null)
                return NotFound();
            var pokemon = await _pokemonReposiotries.GetPokemon(name);
            return Ok(pokemon);
        }

        [HttpGet("{pokeId}/rating")]
        [ProducesResponseType(200, Type = typeof(decimal))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonRating(int pokeId)
        {
            if (!await _pokemonReposiotries.PokemonExists(pokeId))
                return NotFound();
            var rating = await _pokemonReposiotries.GetPokemonRating(pokeId);
            return Ok(rating);
        }

        [HttpPost]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> CreatePokemon([FromQuery] int ownerId, [FromQuery] int categoryID, [FromBody] PokemonResponseDto pokemoncreate)
        {
            if (pokemoncreate == null)
            {
                return BadRequest(ModelState);
            }

            var pokemons = _pokemonReposiotries.GetPokemons().Result.FirstOrDefault(o => o.Name.Trim().ToUpper() == pokemoncreate.Name.TrimEnd().ToUpper());

            if (pokemons != null)
            {
                ModelState.AddModelError("", "pokemon already exist");
                return StatusCode(422, ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var pokemonCat = new Pokemon
            {
                Name = pokemoncreate.Name,
                BirthDate = pokemoncreate.BirthDate,
                CreatedAt = pokemoncreate.CreatedAt,
            };


            if (!await _pokemonReposiotries.CreatePokemon(pokemonCat, ownerId, categoryID))
            {
                ModelState.AddModelError("", "There is an error that happening when saving");
                return StatusCode(500, ModelState);
            }
            ;

            return Ok("Pokemon Created");
        }

        [HttpPut("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> UpdatePokemon(int pokeId, [FromQuery] int ownerId, [FromQuery] int categoryID, [FromBody] PokemonUpdateRequest pokemonUpdate)
        {
            if (pokemonUpdate == null)
            {
                return BadRequest(ModelState);
            }
            if (pokeId != pokemonUpdate.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _pokemonReposiotries.PokemonExists(pokeId))
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var pokemonCat = new Pokemon
            {
                Id = pokemonUpdate.Id,
                Name = pokemonUpdate.Name,
                BirthDate = pokemonUpdate.BirthDate,
                UpdatedAt = DateTime.Now,
            };
            if (!await _pokemonReposiotries.UpdatePokemon(pokemonCat, ownerId, categoryID))
            {
                ModelState.AddModelError("", "There is an error that happening when saving");
                return StatusCode(500, ModelState);
            }
            ;
            return Ok("Pokemon Updated");
        }

        [HttpDelete("{pokeId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeletePokemon(int pokeId)
        {
            if (!await _pokemonReposiotries.PokemonExists(pokeId))
            {
                return NotFound();
            }
            var reviewDel = await _reviewRepository.GetReviewsOfAPokemon(pokeId);
            var pokemonDel = await _pokemonReposiotries.GetPokemon(pokeId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            foreach (var review in reviewDel)
            {
                if (!await _reviewRepository.DeleteReviews(review))
                {
                    ModelState.AddModelError("", "There is an error that happened when saving");
                    return StatusCode(500, ModelState);
                }
            }

            if (!await _pokemonReposiotries.DeletePokemon(pokemonDel))
            {
                ModelState.AddModelError("", "There is an error that happened when saving");
                return StatusCode(500, ModelState);
            }

            return Ok("Pokemon Deleted");
        }
    }
}
