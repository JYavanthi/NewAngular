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
    public class PkgIncludeController : ControllerBase
    {
        private readonly IPkgInclude _pkgInclude;
        private readonly Sanchar6tDbContext _context;
  
        public PkgIncludeController(IPkgInclude pkgInclude, Sanchar6tDbContext context)
        {
            this._pkgInclude = pkgInclude;
            this._context = context;
        }

        [HttpGet("GetpkgIncludeByID")]
        public async Task<CommonRsult> GetpkgIncludeByID(int PkgImageID)
        {
            return await _pkgInclude.GetPkgIncludeByID(PkgImageID);
        }
        [HttpGet("GetpkgIncludeByPackageID")]
        public async Task<CommonRsult> GetpkgIncludeByPackageID(int PackageID)
        {
            return await _pkgInclude.GetPkgIncludeByPackageID(PackageID);
        }

        [HttpGet("GetpkgInclude")]
        public async Task<CommonRsult> GetpkgInclude()
        {
            return await _pkgInclude.GetPkgInclude();
        }

        [HttpPost("SavepkgInclude")]
        public async Task<CommonRsult> PkgInclude(EPkgInclude pkgInclude)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkgInclude.SavePkgInclude(pkgInclude);
            return data1;
        }
    }
}
