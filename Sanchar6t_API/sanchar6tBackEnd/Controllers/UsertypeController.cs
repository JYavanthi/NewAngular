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
    public class UsertypeController : ControllerBase  
    {
        private readonly IUsertype _usertype;
        private readonly Sanchar6tDbContext _context;

        public UsertypeController(IUsertype usertype, Sanchar6tDbContext context)
        {
            this._usertype = usertype;
            this._context = context;
        }
        [HttpGet("GetUsertype")]
        public async Task<IActionResult> GetUsertype()
        {
            var data = await _usertype.GetUsertype();
            return Ok(data);
        }
        [HttpPost("PostUsertype")]
        public async Task<CommonRsult> SaveUsertype(EUsertype eUsertype)
        {
            var data1 = await _usertype.SaveUsertype(eUsertype);
            return data1;
        }
    }
}
