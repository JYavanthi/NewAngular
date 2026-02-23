using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgImpNotes
    {
        Task<CommonRsult> SavePkgImpNotes(EPkgImpNotes pkgImpNotes);
        Task<CommonRsult> GetpkgImpNotes();
        Task<CommonRsult> GetpkgImpNotesByID(int PkgImpNotesID);
        Task<CommonRsult> GetpkgImpNotesByPackageID(int PackageID);
    }
}  