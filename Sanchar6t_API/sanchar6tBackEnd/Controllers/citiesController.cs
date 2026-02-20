using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class citiesController : ControllerBase
    {
        private readonly Icities _icities;
        private readonly Sanchar6tDbContext _context;

        public citiesController(Icities icities, Sanchar6tDbContext context)
        {
            this._icities = icities;
            this._context = context;
        }
        [HttpPost("Postcities")]
        public async Task<CommonRsult> Savecities(Ecities ecities)
        {
            var data1 = await _icities.Savecities(ecities);
            return data1;
        }
        [HttpGet("Getcities")]
        public async Task<IActionResult> Getcities()
        {
            var data = await _icities.Getcities();
            return Ok(data);
        }

        //[HttpGet("Getcities")]
        //public async Task<IActionResult> Getcities(int pageIndex = 0, int pageSize = 10)
        //{
        //    // Pass pageIndex and pageSize to the repository method for pagination
        //    var data = await _icities.Getcities(pageIndex, pageSize);
        //    return Ok(data);
        //}

    }
}
