using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Services
{
    public interface IState
    {
        Task<CommonRsult> GetStatebycountryID(int countryID);
        Task<CommonRsult> GetStateByID(int stateID);
        Task<CommonRsult> GetState();
        Task<CommonRsult> SaveState(EState eState);


    }
}
