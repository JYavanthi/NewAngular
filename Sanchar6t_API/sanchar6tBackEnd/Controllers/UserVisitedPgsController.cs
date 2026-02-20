using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserVisitedPgsController : ControllerBase
    {
        private readonly IUserVisitedPgs _userVisitedPgs;
        private readonly Sanchar6tDbContext _context;

        public UserVisitedPgsController(IUserVisitedPgs userVisitedPgs, Sanchar6tDbContext context)
        {
            this._userVisitedPgs = userVisitedPgs;
            this._context = context;
        }
        //[HttpGet("GetUserVisitedPgs")]
        //public IActionResult GetVisitedPgsdata()
        //{
        //    var data = _context.VwUserVisitedPgs.ToList();
        //    return Ok(data);
        //}
        /*[HttpPost("PostUserVisitedPgs")]
        public async Task<CommonRsult> SaveUserVisitedPgs(EUserVisitedPgs userVisitedPgs)
        {
            var data1 = await _userVisitedPgs.SaveUserVisitedPgs(userVisitedPgs);
            return data1;
        }*/
    }
}
