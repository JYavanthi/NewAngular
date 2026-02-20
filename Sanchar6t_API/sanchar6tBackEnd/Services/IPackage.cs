using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPackage
    {
        Task<CommonRsult> SavePackagedetails(EPackage package);
        Task<CommonRsult> GetPackageDtls();
        Task<CommonRsult> GetPackageDtlByID(int PackageID);
    }
}
 