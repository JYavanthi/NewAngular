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
    public class ServiceDtlsController : ControllerBase
    {
        private readonly IServiceDtls _serviceDtls;
        private readonly Sanchar6tDbContext _context;

        public ServiceDtlsController(IServiceDtls serviceDtls, Sanchar6tDbContext context)
        {
            this._serviceDtls = serviceDtls;
            this._context = context;
        }

        [HttpGet("GetServiceDtlsByID")]
        public async Task<CommonRsult> GetServiceDtlsByID(int PkgImageID)
        {
            return await _serviceDtls.GetServiceDtlsByID(PkgImageID);
        }
        [HttpGet("GetpkgImageDtlByPackageID")]
        public async Task<CommonRsult> GetServiceDtlsByPackageID(int PackageID)
        {
            return await _serviceDtls.GetServiceDtlsByPackageID(PackageID);
        }

        [HttpGet("GetServiceDtls")]
        public IActionResult GetServiceDtls()
        {
            var data = _context.Packages.ToList();
            return Ok(data);
        }
        [HttpPost("SaveServiceDtls")]
        public async Task<CommonRsult> ServiceDtls(EServiceDtls serviceDtls)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _serviceDtls.SaveServiceDtls(serviceDtls);
            return data1;
        }
    }
}