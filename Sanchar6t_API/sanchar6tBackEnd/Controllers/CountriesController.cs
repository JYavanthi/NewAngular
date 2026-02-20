using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class countriesController : ControllerBase
    {
        private readonly Icountries _country;
        private readonly Sanchar6tDbContext _context;

        public countriesController(Icountries country, Sanchar6tDbContext context)
        {
            this._country = country;
            this._context = context;

        }
        [HttpGet("Getcountries")]
        public async Task<IActionResult> Getcountriestails()
        {
            var data = await _country.GetCountries();
            return Ok(data);
        }
        [HttpGet("GetCountryByID")]
        public async Task<IActionResult> GetCountriesdetailByID(int CountryID)
        {
            var data = await _country.GetCountryByID(CountryID);
            return Ok(data);
        }
        [HttpPost("Postcountries")]
        public async Task<CommonRsult> Savecountries(Ecountries ecountries)
        {
            var data = await _country.Savecountries(ecountries);
            return data;
        }
    }
}
