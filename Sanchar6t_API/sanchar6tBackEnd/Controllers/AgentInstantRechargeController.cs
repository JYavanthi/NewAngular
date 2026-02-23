using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [ApiController]
    [Route("api/agent-instant-recharge")]
    public class AgentInstantRechargeController : ControllerBase
    {
        private readonly IAgentInstantRechargeRepository _repository;

        public AgentInstantRechargeController(
            IAgentInstantRechargeRepository repository)
        {
            _repository = repository;
        }

        // ================= CREATE INSTANT RECHARGE =================
        [HttpPost("create")]
        public IActionResult CreateRecharge(
            [FromBody] EAgentInstantRechargeDtl model)
        {
            if (model == null)
                return BadRequest("Invalid request data");

            if (model.Amount <= 0)
                return BadRequest("Amount must be greater than zero");

            model.Status = "Pending";

            var rechargeId = _repository.Create(model);

            return Ok(new
            {
                InstantRechargeId = rechargeId,
                Status = "Pending",
                Message = "Instant recharge created successfully"
            });
        }


        
        [HttpGet("get-by-id/{instantRechargeId}")]
        [AllowAnonymous]
        public IActionResult GetRechargeById(int instantRechargeId)
        {
            if (instantRechargeId <= 0)
                return BadRequest("Invalid InstantRechargeId");

            var recharge = _repository.GetById(instantRechargeId);

            if (recharge == null)
                return NotFound("Instant recharge not found");

            return Ok(recharge);
        }


        // ================= UPDATE STATUS =================
        [HttpPost("update-status")]
        [AllowAnonymous]
        public IActionResult UpdateRechargeStatus(
            [FromBody] EAgentInstantRechargeDtl model)
        {
            if (model == null)
                return BadRequest("Invalid request data");

            if (model.InstantRechargeId <= 0)
                return BadRequest("Invalid InstantRechargeId");

            if (string.IsNullOrEmpty(model.Status))
                return BadRequest("Status is required");

            _repository.UpdateStatus(model);

            return Ok(new
            {
                Message = "Recharge status updated successfully"
            });
        }
    }
}
