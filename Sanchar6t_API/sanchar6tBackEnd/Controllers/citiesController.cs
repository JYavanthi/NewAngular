using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CitiesController : ControllerBase
    {
        private readonly Icities _cities;

        public CitiesController(Icities cities)
        {
            _cities = cities;
        }

        
        [HttpGet("GetCitiesByState")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCitiesByState([FromQuery] int stateId)
        {
            if (stateId <= 0)
                return BadRequest("StateId is required");

            var result = await _cities.GetCitiesByStateIdAsync(stateId);
            return Ok(result);
        }

        // ✅ GET ALL CITIES (OPTIONAL)
        [HttpGet("GetCities")]
        public async Task<IActionResult> GetCities()
        {
            var result = await _cities.GetCitiesAsync();
            return Ok(result);
        }

        // ✅ SAVE / UPDATE CITIES
        [HttpPost("SaveCities")]
        public async Task<IActionResult> SaveCities([FromBody] List<Ecities> cities)
        {
            if (cities == null || cities.Count == 0)
                return BadRequest("City list is empty");

            var result = await _cities.SaveCitiesAsync(cities);
            return Ok(result);
        }
    }
}
