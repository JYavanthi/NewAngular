using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PkgImageDtlsController : ControllerBase
    {
        private readonly IPkgImageDtls _pkgImageDtls;
        private readonly Sanchar6tDbContext _context;

        public PkgImageDtlsController(IPkgImageDtls pkgImageDtls, Sanchar6tDbContext context)
        {
            this._pkgImageDtls = pkgImageDtls;
            this._context = context;
        }
        [HttpGet("GetpkgImageDtls")]
        public async Task<CommonRsult> GetPkgImageDtls()
        {
            return await _pkgImageDtls.GetPkgImageDtls();
        }
        [HttpGet("GetpkgImageDtlByID")]
        public async Task<CommonRsult> GetPkgImageDtlByID(int PkgImageID)
        {
            return await _pkgImageDtls.GetPkgImageDtlByID(PkgImageID);
        }
        [HttpGet("GetpkgImageDtlByPackageID")]
        public async Task<CommonRsult> GetPkgImageDtlByPackageID(int PackageID)
        {
            return await _pkgImageDtls.GetPkgImageDtlByPackageID(PackageID);
        }
        [HttpPost("SavePkgImageDtls")]
        public async Task<CommonRsult> PkgImageDtls(EPkgImageDtls pkgImageDtls)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkgImageDtls.SavePkgImageDtls(pkgImageDtls);
            return data1;
        }
    }
}