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
    public class SavedPassengerDtlsController : ControllerBase
    {
        private readonly ISavedPassengerDtls _savedPassengerDtls;
        private readonly Sanchar6tDbContext _context;

        public SavedPassengerDtlsController(ISavedPassengerDtls savedPassengerDtls, Sanchar6tDbContext context)
        {
            this._savedPassengerDtls = savedPassengerDtls;
            this._context = context;
        }
        [HttpGet("GetSavedPassengerDtlsbyUserID")]
        public async Task<IActionResult> GetSavedPassengerDtlsbyUserID(int UserID)
        {
            var data = await _savedPassengerDtls.GetSavedPassengerDtlsbyUserID(UserID);
            return Ok(data);
        }
        [HttpPost("SaveOrUpdate")]
        public async Task<IActionResult> SaveOrUpdate([FromBody] SavedPassengerDtl model)
        {
            var result = await _savedPassengerDtls.SaveOrUpdatePassengerDetails(model);
            return Ok(result);
        }

    }
}
