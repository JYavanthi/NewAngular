using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgOffer
    {
        Task<CommonRsult> SavepkgOffer(EPkgOffer pkgOffer);
        Task<CommonRsult> GetpkgOffer();
        Task<CommonRsult> GetpkgOfferByID(int pkgOfferID);
        Task<CommonRsult> GetpkgOfferByPackageID(int PackageID);
    }
}
 