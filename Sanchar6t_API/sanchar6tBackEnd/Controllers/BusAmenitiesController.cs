using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusAmenitiesController : ControllerBase
    {
        private readonly IBusAmenities _busAmenities;
        private readonly Sanchar6tDbContext context;

        public BusAmenitiesController(IBusAmenities busAmenities, Sanchar6tDbContext context)
        {
            this._busAmenities = busAmenities;
            this.context = context;
        }
        [HttpPost("PostBusAmenities")]
        public async Task<CommonRsult> SaveBusAmenities(EBusAmenities eBusAmenities)
        {
            var data1 = await _busAmenities.SaveBusAmenities(eBusAmenities);
            return data1;
        }
        [HttpGet("GetBusAmenities")]
        public async Task<IActionResult> GetBusAmenitiesdetails()
        {
            var data = await _busAmenities.GetBusAmenities();
            return Ok(data);
        }
    }
}
