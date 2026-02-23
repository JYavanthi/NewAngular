using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Data;

namespace sanchar6tBackEnd.Services
{
    public interface Icities
    {
        Task<CommonRsult> SaveCitiesAsync(List<Ecities> cities);
        Task<CommonRsult> GetCitiesAsync();
        Task<CommonRsult> GetCitiesByStateIdAsync(int stateId);
    }
}
