using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IUserLogs
    {
        Task<CommonRsult> SaveUserLogs(EUserLogs userLogs);
        Task<CommonRsult> GetUserLogs();

    }
}
 