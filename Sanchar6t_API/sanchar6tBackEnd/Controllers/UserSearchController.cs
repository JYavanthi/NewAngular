using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserSearchController : ControllerBase
    {
        private readonly IUserSearch _userSearch;
        private readonly Sanchar6tDbContext _context;

        public UserSearchController(IUserSearch userSearch, Sanchar6tDbContext context)
        {
            this._userSearch = userSearch;
            this._context = context;
        }
        [HttpGet("GetPayment")]
        public async Task<IActionResult> GetUserSearch()
        {
            var data = await _userSearch.GetUserSearch();
            return Ok(data);
        }
        [HttpPost("PostPayment")]
        public async Task<CommonRsult> SaveUserSearch(EUserSearch eUserSearch)
        {
            var data1 = await _userSearch.SaveUserSearch(eUserSearch);
            return data1;
        }
    }
}
