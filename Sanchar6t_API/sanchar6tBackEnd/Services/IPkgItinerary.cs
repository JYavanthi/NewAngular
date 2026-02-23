using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgItinerary
    {
        Task<CommonRsult> SavePkgItinerary(EPkgItinerary pkgItinerary);
        Task<CommonRsult> GetPkgItinerary();
        Task<CommonRsult> GetPkgItineraryByID(int PkgItineraryID);
        Task<CommonRsult> GetPkgItineraryByPackageID(int PackageID);
    }
} 