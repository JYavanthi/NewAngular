using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PkgHighlightController : ControllerBase
    {
        private readonly IPkgHighlight _pkghighlight;
        private readonly Sanchar6tDbContext _context;

        public PkgHighlightController(IPkgHighlight ipkghighlight, Sanchar6tDbContext context)
        {
            this._pkghighlight = ipkghighlight;
            this._context = context;
        }
        [HttpGet("Getpkghighlight")]

        public async Task<CommonRsult> GetPkgHighlight()
        {
            return await _pkghighlight.GetPkgHighlight();
        }
        [HttpGet("GetpkghighlightByPackageID")]

        public async Task<CommonRsult> GetPkgHighlightByID(int PackageID)
        {
            return await _pkghighlight.GetPkgHighlightByID(PackageID);
        }
        [HttpPost("SavePostpackage")]
        public async Task<CommonRsult> PkgHighlight(EPkgHighlight pkghighlight)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkghighlight.SavePkgHighlight(pkghighlight);
            return data1;
        }
    }
}
  