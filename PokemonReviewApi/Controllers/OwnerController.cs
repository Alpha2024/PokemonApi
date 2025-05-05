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
    public class OwnerController : ControllerBase
    {
        private readonly IOwnerRepository _ownerRepository;

        public OwnerController(IOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]

        public async Task<IActionResult> GetOwners()
        {
            var owners = await _ownerRepository.GetOwners();
            if (owners == null)
            {
                return BadRequest();
            }
            return Ok(owners);
        }

        [HttpGet("{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Owner))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = await _ownerRepository.GetOwner(ownerId);
            if (owner == null)
            {
                return BadRequest();
            }
            return Ok(owner);
        }

        [HttpGet("{pokemonId}/owners")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Owner>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetOwnersOfAPokemon(int pokemonId)
        {
            if (!await _ownerRepository.OwnerExists(pokemonId))
            {
                return NotFound();
            }
            var owners = await _ownerRepository.GetOwnersOfAPokemon(pokemonId);
            if (owners == null)
            {
                return BadRequest();
            }
            return Ok(owners);
        }

        [HttpGet("{ownerId}/pokemons")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Pokemon>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetPokemonsOfAnOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var pokemons = await _ownerRepository.GetPokemonsOfAnOwner(ownerId);
            if (pokemons == null)
            {
                return BadRequest();
            }
            return Ok(pokemons);
        }

        [HttpPost("createOwner")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateOwner([FromBody] OwnerAddRequestDto ownerAddRequest)
        {
            if (ownerAddRequest == null)
            {
                return BadRequest(ModelState);
            }

            var owner = _ownerRepository.GetOwners().Result.FirstOrDefault(o => o.FirstName.Trim().ToUpper() == ownerAddRequest.FirstName.TrimEnd().ToUpper() && o.LastName.Trim().ToUpper() == ownerAddRequest.LastName.TrimEnd().ToUpper());

            if (owner != null)
            {
                ModelState.AddModelError("", "Owner already exists");
                return StatusCode(422, ModelState);
            }
            var ownernew = new Owner()
            {
                FirstName = ownerAddRequest.FirstName,
                LastName = ownerAddRequest.LastName,
                Gym = ownerAddRequest.Gym,
                CreatedAt = DateTime.Now
            };
            if (!await _ownerRepository.CreateOwner(ownernew))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {ownerAddRequest.FirstName}");
                return StatusCode(500, ModelState);
            }
            return Ok("Owner created ");
        }

        [HttpPut("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateOwner(int ownerId, [FromBody] OwnerUpdateRequestDto ownerUpdateRequest)
        {
            if (ownerUpdateRequest == null)
            {
                return BadRequest(ModelState);
            }
            if (ownerId != ownerUpdateRequest.Id)
            {
                return BadRequest(ModelState);
            }
            var owner = await _ownerRepository.GetOwner(ownerId);
            if (owner == null)
            {
                return NotFound();
            }
            owner.FirstName = ownerUpdateRequest.FirstName;
            owner.LastName = ownerUpdateRequest.LastName;
            owner.Gym = ownerUpdateRequest.Gym;
            owner.UpdatedAt = DateTime.Now;
            if (!await _ownerRepository.UpdateOwner(owner))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {ownerUpdateRequest.FirstName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

        [HttpDelete("{ownerId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteOwner(int ownerId)
        {
            if (!await _ownerRepository.OwnerExists(ownerId))
            {
                return NotFound();
            }
            var owner = await _ownerRepository.GetOwner(ownerId);
            if (!await _ownerRepository.DeleteOwner(owner))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {owner.FirstName}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }
    }
}
