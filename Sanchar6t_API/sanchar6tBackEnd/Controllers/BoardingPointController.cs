using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BoardingPointController : ControllerBase
    {
        private readonly IBoardingPoint _boardingPoint;
        private readonly Sanchar6tDbContext _context;



        public BoardingPointController(IBoardingPoint boardingPoint, Sanchar6tDbContext context)
        {
            this._boardingPoint = boardingPoint;
            this._context = context;
        }
        [HttpPost("PostBoardingPoint")]
        public async Task<CommonRsult> SaveBoardingPoint(EBoardingPoint eBoardingPoint)
        {
            var data1 = await _boardingPoint.SaveBoardingPoint(eBoardingPoint);
            return data1;
        }
        [HttpGet("GetBoardingPoint")]
        public async Task<IActionResult> GetBoardingPoint()
        {
            var data = await _boardingPoint.GetBoardingPoint();
            return Ok(data);
        }
    }
}
