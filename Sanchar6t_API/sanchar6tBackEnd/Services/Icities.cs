using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Data;

namespace sanchar6tBackEnd.Services
{
    public interface Icities
    {
        Task<CommonRsult> Savecities(Ecities ecities);
        Task<CommonRsult> Getcities();
    }
}
