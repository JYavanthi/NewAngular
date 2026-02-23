using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgImageDtls
    {
        Task<CommonRsult> SavePkgImageDtls(EPkgImageDtls pkgimagedtls);
        Task<CommonRsult> GetPkgImageDtls();
        Task<CommonRsult> GetPkgImageDtlByID(int PkgImageID);
        Task<CommonRsult> GetPkgImageDtlByPackageID(int PackageID);
    }
}

 