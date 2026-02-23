

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AgentDtlsController : ControllerBase
    {
        private readonly Sanchar6tDbContext _context;

        public AgentDtlsController(Sanchar6tDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAgentDtls")]
        [AllowAnonymous]
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

        [HttpGet("GetAgentDtlsByUserId/{userId}")]
        [AllowAnonymous]
        public IActionResult GetAgentDtlsByUserId(int userId)
        {
            var data = _context.VwAgentDtls
                .FirstOrDefault(x => x.UserId == userId);

            if (data == null)
                return NotFound();

            return Ok(data);
        }

        [HttpGet("GetAgentDtlsByAgentId/{agentDtlId}")]
        [AllowAnonymous]
        public IActionResult GetAgentDtlsByAgentId(int agentDtlId)
        {
            var data = _context.VwAgentDtls
                .FirstOrDefault(x => x.AgentDtlId == agentDtlId);

            if (data == null)
                return NotFound();

            return Ok(data);
        }


        [HttpDelete("Delete/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> Delete(int id)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var agent = await _context.AgentDtls
                      .FirstOrDefaultAsync(x => x.AgentDtlId == id);

                if (agent == null)
                {
                    result.Type = "E";
                    result.Message = "Agent Not Found!";
                    return Ok(result);
                }

                _context.AgentDtls.Remove(agent);  
                await _context.SaveChangesAsync();

                result.Type = "S";
                result.Message = "Agent deleted successfully";
                return Ok(result);
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return Ok(result);
        }


        //#################################################################
        // APPROVE / REJECT AGENT
        //#################################################################
        [HttpPost("AgentApproval")]
        [AllowAnonymous]
        public async Task<IActionResult> AgentApproval([FromBody] AgentApprovalRequest model)
        {
            CommonRsult result = new CommonRsult();

            try
            {
                var agent = await _context.AgentDtls
                    .FirstOrDefaultAsync(x => x.AgentDtlId == model.AgentDtlID);

                if (agent == null)
                {
                    result.Type = "E";
                    result.Message = "Agent not found!";
                    return Ok(result);
                }

                agent.Status = model.Status;                  // Approved / Rejected
                agent.ModifiedBy = model.CreatedBy;
                agent.ModifiedDt = DateTime.Now;

                await _context.SaveChangesAsync();

                result.Type = "S";
                result.Message = "Agent status updated successfully";
                return Ok(result);
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

public class AgentApprovalRequest
{
    public int AgentDtlID { get; set; }
    public string Status { get; set; }
    public int CreatedBy { get; set; }
}
