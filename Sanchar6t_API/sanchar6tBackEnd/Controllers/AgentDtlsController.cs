using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentDtlsController : ControllerBase
    {
        private readonly Sanchar6tDbContext _context;

        public AgentDtlsController(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        [HttpGet("GetAgentDtls")]
        public IActionResult GetAgentDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = _context.VwAgentDtls.ToList();
                return Ok(data);
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return Ok(result);
        }
    }
}
