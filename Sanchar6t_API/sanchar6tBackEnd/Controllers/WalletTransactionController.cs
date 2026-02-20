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
    public class WalletTransactionController : ControllerBase
    {
        private readonly IWalletTransaction _walletTransaction;
        private readonly Sanchar6tDbContext _context;

        public WalletTransactionController(IWalletTransaction walletTransaction, Sanchar6tDbContext context)
        {
            this._walletTransaction = walletTransaction;
            this._context = context;
        }


        [HttpGet("GetWalletTransactionByID")]
        public async Task<CommonRsult> GetWalletTransactionByID(int userID)
        {
            return await _walletTransaction.GetWalletTransactionByID(userID);
        }



        [HttpGet("GetWalletTransaction")]
        public IActionResult GetWalletTransaction()
        {
            var data = _context.VwWalletTransactions.ToList();
            return Ok(data);
        }

        [HttpPost("SaveWalletTransaction")]
        public async Task<CommonRsult> WalletTransaction(EWalletTransaction walletTransaction)
        {
            var data1 = await _walletTransaction.SaveWalletTransaction(walletTransaction);
            return data1;
        }

    }
}
