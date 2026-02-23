using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StateController : ControllerBase
    {
        private readonly IState _state;
        private readonly Sanchar6tDbContext _context;

        public StateController(IState state, Sanchar6tDbContext context)
        {
            this._state = state;
            this._context = context;
        }
        [HttpGet("GetState")]
        [AllowAnonymous]
        public async Task<IActionResult> GetState(
     [FromHeader(Name = "Country-Id")] int countryId)
        {
            if (countryId <= 0)
                return BadRequest("Country-Id header is required");

            var data = await _state.GetStatebycountryID(countryId);
            return Ok(data);
        }

        [HttpGet("GetStateByID")]
        public async Task<IActionResult> GetStateByID(int stateID)
        {
            var data = await _state.GetStateByID(stateID);
            return Ok(data);
        }
        [HttpGet("GetStateByCountry/{countryID}")]
        public async Task<IActionResult> GetStatebycountryID(int countryID)
        {
            var data = await _state.GetStatebycountryID(countryID);
            return Ok(data);
        }
        [HttpPost("PostState")]
        public async Task<CommonRsult> SaveState(EState eState)
        {
            var data1 = await _state.SaveState(eState);
            return data1;
        }
    }
}
