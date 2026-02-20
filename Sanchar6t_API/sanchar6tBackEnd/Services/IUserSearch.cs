using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IUserSearch
    {
        Task<CommonRsult> SaveUserSearch(EUserSearch eUserSearch);
        Task<CommonRsult> GetUserSearch();
    }
}
