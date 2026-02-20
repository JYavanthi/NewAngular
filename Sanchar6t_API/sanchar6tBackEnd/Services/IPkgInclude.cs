using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgInclude
    {
        Task<CommonRsult> SavePkgInclude(EPkgInclude pkgInclude);
        Task<CommonRsult> GetPkgInclude();
        Task<CommonRsult> GetPkgIncludeByID(int PkgIncludeID);
        Task<CommonRsult> GetPkgIncludeByPackageID(int PackageID);
    }
} 