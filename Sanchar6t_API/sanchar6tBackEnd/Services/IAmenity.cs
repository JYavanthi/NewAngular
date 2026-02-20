using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IAmenity
    {
        Task<CommonRsult> SaveAmenity(EAmenity eAmenity);

        Task<CommonRsult> GetAmenity();
    }
}
