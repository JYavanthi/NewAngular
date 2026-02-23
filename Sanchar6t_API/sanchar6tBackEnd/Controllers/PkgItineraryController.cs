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
    public class PkgItineraryController : ControllerBase
    {
        private readonly IPkgItinerary _pkgItinerary;
        private readonly Sanchar6tDbContext _context;
  
        public PkgItineraryController(IPkgItinerary pkgItinerary, Sanchar6tDbContext context)
        {
            this._pkgItinerary = pkgItinerary;
            this._context = context;
        }

        [HttpGet("GetpkgItinerary")]
        public async Task<CommonRsult> GetpkgItinerary()
        {
            return await _pkgItinerary.GetPkgItinerary();
        }
        [HttpGet("GetpkgItineraryByID")]
        public async Task<CommonRsult> GetpkgItineraryByID(int PkgImageID)
        {
            return await _pkgItinerary.GetPkgItineraryByID(PkgImageID);
        }
        [HttpGet("GetpkgImageDtlByPackageID")]
        public async Task<CommonRsult> GetPkgImageDtlByPackageID(int PackageID)
        {
            return await _pkgItinerary.GetPkgItineraryByPackageID(PackageID);
        }
        [HttpPost("SavepkgItinerary")]
        public async Task<CommonRsult> PkgItinerary(EPkgItinerary pkgItinerary)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkgItinerary.SavePkgItinerary(pkgItinerary);
            return data1;
        }
    }
}
