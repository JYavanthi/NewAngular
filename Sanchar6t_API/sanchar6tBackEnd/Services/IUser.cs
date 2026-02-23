using Microsoft.AspNetCore.Mvc.ActionConstraints;
using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IUser
    {
        Task<CommonRsult> SaveUser(User user);
        Task<CommonRsult> GetUser();
        Task<CommonRsult> GetUserByID(int userID);
    }
}
 