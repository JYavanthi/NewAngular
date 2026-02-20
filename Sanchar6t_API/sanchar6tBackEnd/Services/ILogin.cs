using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface ILogin
    {
        Task<CommonRsult> AuthenticateUser(string Email,string MoblieNo, string password);
    }
}
