using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using User = sanchar6tBackEnd.Data.Entities.User;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Sanchar6tDbContext _context;
        private readonly IUser _user;

        public UserController(Sanchar6tDbContext context,  IUser user)
        {
            _context = context;
            this._user = user;
        }


        [HttpGet("GetUser")]
        
        public async  Task<CommonRsult> GetData()
        {
            return await _user.GetUser();
        }
        [HttpGet("GetUserByID")]
        
        public async  Task<CommonRsult> GetData(int userID)
        {
            return await _user.GetUserByID(userID);
        }

        [HttpPost("PostUser")]
        [AllowAnonymous]
        public async Task<CommonRsult> SaveUser(User user)
        {
            var data = await _user.SaveUser(user);
            return data;
        }
    }
}