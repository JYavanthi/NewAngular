using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusOperatorController : ControllerBase
    {
        private readonly IBusOperator _busOperator;
        private readonly Sanchar6tDbContext _context;

        public BusOperatorController(IBusOperator busOperator, Sanchar6tDbContext context)
        {
            this._busOperator = busOperator;
            this._context = context;
        }
        [HttpPost("PostBusOperator")]
        public async Task<CommonRsult> SaveBusOperator(EBusOperator eBusOperator)
        {
            var data1 = await _busOperator.SaveBusOperator(eBusOperator);
            return data1;
        }
        [HttpGet("GetBusOperator")]
        public async Task<IActionResult> GetBusOperatordetails()
        {
            var data = await _busOperator.GetBusOperator();
            return Ok(data);
        }
    }
}
