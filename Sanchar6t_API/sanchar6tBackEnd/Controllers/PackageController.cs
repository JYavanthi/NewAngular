using System.Runtime.CompilerServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PackageController : ControllerBase
    {
        private readonly IPackage _package;
        private readonly Sanchar6tDbContext _context;

        public PackageController(IPackage package, Sanchar6tDbContext context)
        {
            this._package = package;
            this._context = context;
        }
        [HttpGet("GetpackageDetails")]
        public async Task<CommonRsult> GetPackageDtls()
        {
            return await _package.GetPackageDtls();
        }
        [HttpGet("GetpackageDtlByID")]
        public async Task<CommonRsult> GetPackageDtlByID(int PackageID)
        {
            return await _package.GetPackageDtlByID(PackageID);
        }
        [HttpPost("Postpackage")]
        public async Task<CommonRsult> Packagedetails(EPackage package)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _package.SavePackagedetails(package);  
            return data1;
        }
    }
}