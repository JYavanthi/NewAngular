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
    public class PkgImpNotesController : ControllerBase
    {  
        private readonly IPkgImpNotes _pkgImpNotes;
        private readonly Sanchar6tDbContext _context;

        public PkgImpNotesController(IPkgImpNotes pkgImpNotes, Sanchar6tDbContext context)
        {
            this._pkgImpNotes = pkgImpNotes;
            this._context = context;
        }

        [HttpGet("GetPkgImpNotes")]
        public async Task<CommonRsult> GetPkgImpNotes()
        {
            return await _pkgImpNotes.GetpkgImpNotes();
        }
        [HttpGet("GetPkgImpNotesByID")]
        public async Task<CommonRsult> GetpkgImpNotesByID(int PkgImpNotesID)
        {
            return await _pkgImpNotes.GetpkgImpNotesByID(PkgImpNotesID);
        }

        [HttpGet("GetPkgImpNotesByPackageID")]
        public async Task<CommonRsult> GetPkgImpNotesByPackageID(int PackageID)
        {
            return await _pkgImpNotes.GetpkgImpNotesByPackageID(PackageID);
        }

        [HttpPost("SavePkgImpNotes")]
        public async Task<CommonRsult> PkgImpNotes(EPkgImpNotes pkgImpNotes)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _pkgImpNotes.SavePkgImpNotes(pkgImpNotes);
            return data1;
        }
    }
}
