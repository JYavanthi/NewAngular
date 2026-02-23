/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWallet _wallet;
        private readonly Sanchar6tDbContext _context;

        public WalletController(IWallet wallet, Sanchar6tDbContext context)
        {
            this._wallet = wallet;
            this._context = context;
        }


         [HttpGet("GetWalletByID")]
        public async Task<CommonRsult> GetWalletByID(int WalletID)
        {
            return await _wallet.GetWalletByID(WalletID);
        }

        [HttpGet("GetWallet")]
        public async Task<CommonRsult> GetWallet(int UserID)
        {
            return await _wallet.GetWallet(UserID);
        }
        [HttpPost("SaveWallet")]
        public async Task<CommonRsult> Wallet(EWallet wallet)
        {
            var data1 = await _wallet.SaveWallet(wallet);
            return data1; 
        }
    }
}
*/