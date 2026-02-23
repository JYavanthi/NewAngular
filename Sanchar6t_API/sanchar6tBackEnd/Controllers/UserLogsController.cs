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
    public class UserLogsController : ControllerBase
    {
        private readonly IUserLogs _userLogs;
        private readonly Sanchar6tDbContext _context;
          
        public UserLogsController(IUserLogs userLogs, Sanchar6tDbContext context)
        {
            this._userLogs = userLogs;
            this._context = context;
        }
        [HttpGet("GetUserLogs")]
        public async Task<IActionResult> GetUserlogsdata()
        {
            var data = await _userLogs.GetUserLogs(); 
            return Ok(data);
        }

        [HttpPost("PostUserLogs")]
        public async Task<CommonRsult> SaveUserLogs(EUserLogs userLogs)
        {
            var data1 = await _userLogs.SaveUserLogs(userLogs);
            return data1;
        }
    }
}
