using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IServiceDtls
    {
        Task<CommonRsult> SaveServiceDtls(EServiceDtls serviceDtls);
        Task<CommonRsult> GetServiceDtls();
        Task<CommonRsult> GetServiceDtlsByID(int ServiceDtlsID);
        Task<CommonRsult> GetServiceDtlsByPackageID(int PackageID);
    }
}
 