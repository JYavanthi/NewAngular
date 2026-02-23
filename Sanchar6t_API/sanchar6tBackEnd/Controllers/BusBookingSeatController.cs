using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusBookingSeatController : ControllerBase
    {
        private readonly IBusBookingSeat _busBookingSeat;
        private readonly Sanchar6tDbContext _context;

        public BusBookingSeatController(IBusBookingSeat busBookingSeat, Sanchar6tDbContext context)
        {
            this._busBookingSeat = busBookingSeat;
            this._context = context;
        }
        [HttpGet("GetBusBookingSeatByID")]
        [AllowAnonymous]
        public async Task<CommonRsult> GetBookingSeatDtlByID(int BookingseatID)
        {
            return await _busBookingSeat.GetBookingSeatDtlByID(BookingseatID);
        } 
        [HttpGet("cc")]
        [AllowAnonymous]
        public async Task<CommonRsult> GetBookingSeatDtl()
        {
            return await _busBookingSeat.GetBookingSeatDtls();
        }

        [HttpGet("GetBusBookingSeat")]
        [AllowAnonymous]
        public async Task<CommonRsult> GetBookingSeatDtlsForAngular()
        {
            return await _busBookingSeat.GetBookingSeatDtls();
        }

        [HttpPost("SaveBusBookingSeat")]
        [AllowAnonymous]
        public async Task<CommonRsult> BookingSeatdetails([FromBody] EBusBookingSeat busBookingSeat)
        {
            var userType = User.FindFirst("UserType")?.Value;

            // 🔥 IMPORTANT: UserType is NULL because AllowAnonymous
            // So this condition is SAFE
            if (userType == "User")
            {
                return new CommonRsult
                {
                    Message = "Access denied: Only admins or agent can add details",
                    Type = "E"
                };
            }

            var data1 = await _busBookingSeat.SaveBookingSeatdetails(busBookingSeat);
            return data1;
        }

    }
}  
