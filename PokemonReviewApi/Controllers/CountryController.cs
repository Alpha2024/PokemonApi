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
    public class CountryController : ControllerBase
    {
        private readonly ICountryRepository _countryRepository;

        public CountryController(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Country>))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetAllCountries()
        {
            var countries = await _countryRepository.GetCountries();
            if (countries == null)
            {
                return BadRequest();
            }

            return Ok(countries);
        }


        [HttpGet("{countryId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        public async Task<IActionResult> GetCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var country = await _countryRepository.GetCountry(countryId);
            if (country == null)
            {
                return BadRequest();
            }
            return Ok(country);
        }

        [HttpGet("country/{ownerId}")]
        [ProducesResponseType(200, Type = typeof(Country))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> GetCountryByOwner(int ownerId)
        {
            if (!await _countryRepository.CountryExists(ownerId))
            {
                return NotFound();
            }
            var country = await _countryRepository.GetCountryByOwner(ownerId);
            if (country == null)
            {
                return BadRequest();
            }
            return Ok(country);
        }

        [HttpGet("owners/{countryId}")]
        public async Task<IActionResult> GetOwnersFromACountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var owners = await _countryRepository.GetOwnersFromACountry(countryId);
            if (owners == null)
            {
                return BadRequest();
            }
            return Ok(owners);
        }

        [HttpPost("createCountry")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> CreateCountry([FromBody] CountryAddRequestDto countryAddRequest)
        {

            if (countryAddRequest == null)
            {
                return BadRequest(ModelState);
            }

            var country = _countryRepository.GetCountries().Result.Where(c => c.Name.Trim().ToUpper() == countryAddRequest.Name.TrimEnd().ToUpper()).FirstOrDefault();

            if (country != null)
            {
                ModelState.AddModelError("", "Country already exists");
                return BadRequest(ModelState);
            }
            var countryMap = new Country()
            {
                Name = countryAddRequest.Name,
                CreatedAt = DateTime.Now,
            };

            if (!await _countryRepository.CreateCountry(countryMap))
            {
                ModelState.AddModelError("", $"Something went wrong when saving the record {countryAddRequest.Name}");
                return StatusCode(500, ModelState);
            }

            return Ok("Country created successfully");


        }

        [HttpPut("updateCountry/{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> UpdateCountry(int countryId, [FromBody] CountryUpdateRequestDto countryUpdateRequest)
        {
            if (countryUpdateRequest == null)
            {
                return BadRequest(ModelState);
            }
            if (countryId != countryUpdateRequest.Id)
            {
                return BadRequest(ModelState);
            }
            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }

            var countryMap = new Country()
            {
                Id = countryUpdateRequest.Id,
                Name = countryUpdateRequest.Name,
                UpdatedAt = DateTime.Now
            };
            if (!await _countryRepository.UpdateCountry(countryMap))
            {
                ModelState.AddModelError("", $"Something went wrong when updating the record {countryUpdateRequest.Name}");
                return StatusCode(500, ModelState);
            }
            return Ok("Country updated successfully");
        }

        [HttpDelete("deleteCountry/{countryId}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> DeleteCountry(int countryId)
        {
            if (!await _countryRepository.CountryExists(countryId))
            {
                return NotFound();
            }
            var country = await _countryRepository.GetCountry(countryId);
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            if (!await _countryRepository.DeleteCountry(country))
            {
                ModelState.AddModelError("", $"Something went wrong when deleting the record {country.Name}");
                return StatusCode(500, ModelState);
            }
            return NoContent();
        }

    }
}
