using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AreaController : ControllerBase
    {
        private readonly IArea _area;
        private readonly Sanchar6tDbContext _context;

        public AreaController(IArea area, Sanchar6tDbContext context)
        {
            this._area = area;
            this._context = context;
        }
        [HttpPost("PostArea")]
        public async Task<CommonRsult> SaveArea(EArea eArea)
        {
            var data1 = await _area.SaveArea(eArea);
            return data1;
        }
        [HttpGet("GetArea")]
        public async Task<IActionResult> GetArea()
        {
            var data = await _area.GetArea();
            return Ok(data);
        }
    }
}
