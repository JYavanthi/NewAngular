using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PkgOfferController : ControllerBase
    {
        private readonly IPkgOffer _pkgOffer;
        private readonly Sanchar6tDbContext _context;  

        public PkgOfferController(IPkgOffer pkgOffer, Sanchar6tDbContext context)
        {
            this._pkgOffer = pkgOffer;
            this._context = context;
        }
        [HttpGet("GetPkgofferByID")]
        public async Task<CommonRsult> GetpkgOfferByID(int PkgOfferID)
        {
            return await _pkgOffer.GetpkgOfferByID(PkgOfferID);
        }
        [HttpGet("GetpkgOfferByPackageID")]
        public async Task<CommonRsult> GetpkgOfferByPackageID(int PackageID)
        {
            return await _pkgOffer.GetpkgOfferByPackageID(PackageID);
        }

        [HttpGet("GetpkgOffer")]
        public IActionResult GetpkgOffer()
        {
            var data = _context.VwPkgOffers.ToList();
            return Ok(data);
        }
        [HttpPost("SavepkgOffer")]
        public async Task<CommonRsult> PkgOffer(EPkgOffer pkgOffer)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkgOffer.SavepkgOffer(pkgOffer);
            return data1;
        }
    }
}
