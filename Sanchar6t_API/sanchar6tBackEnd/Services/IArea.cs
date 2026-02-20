using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IArea
    {
        Task<CommonRsult> SaveArea(EArea eArea);
        Task<CommonRsult> GetArea();
    }
}
