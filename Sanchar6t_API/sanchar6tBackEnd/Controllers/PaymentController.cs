using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPayment _payment;
        private readonly Sanchar6tDbContext _context;

        public PaymentController(IPayment payment, Sanchar6tDbContext context)
        {
            this._payment = payment;
            this._context = context;
        }

        [HttpGet("GetPayment")]
        [AllowAnonymous]
        public IActionResult GetPayment()
        {
            var data = _context.VwPayments.ToList();
            return Ok(data);
        }

        [HttpGet("GetTotalAmount")]
        [AllowAnonymous]
        public IActionResult GetTotalAmount()
        {
            // Sum all Amount values from Payment table/view
            var totalAmount = _context.VwPayments.Sum(x => x.Amount);

            return Ok(totalAmount);
        }

        [HttpPost("SavePayment")]
        [AllowAnonymous]
        public async Task<CommonRsult> Payment(EPayment ePayment)
        {
            var data1 = await _payment.SavePayment(ePayment);
            return data1;
        }
    }
}
