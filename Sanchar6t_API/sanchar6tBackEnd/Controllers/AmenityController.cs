using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AmenityController : ControllerBase
    {
        private readonly IAmenity _amenity;
        private readonly Sanchar6tDbContext _context;

        public AmenityController(IAmenity amenity, Sanchar6tDbContext context)
        {
            this._amenity = amenity;
            this._context = context;
        }

        [HttpPost("PostAmenity")]
        public async Task<CommonRsult> SaveAmenity(EAmenity eAmenity)
        {
            var data1 = await _amenity.SaveAmenity(eAmenity);
            return data1;
        }
        [HttpGet("GetAmenity")]
        public async Task<IActionResult> GetAmenitydetails()
        {
            var data = await _amenity.GetAmenity();
            return Ok(data);
        }
    }
}
