using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IUsertype
    {
        Task<CommonRsult> SaveUsertype(EUsertype eUsertype);
        Task<CommonRsult> GetUsertype();
    }
}
  