using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBusAmenities
    {
        Task<CommonRsult> SaveBusAmenities(EBusAmenities eBusAmenities);

        Task<CommonRsult> GetBusAmenities();
    }
}
