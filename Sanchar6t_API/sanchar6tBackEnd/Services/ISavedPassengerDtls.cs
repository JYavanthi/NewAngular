using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Services
{
    public interface ISavedPassengerDtls
    {
        Task<CommonRsult> GetSavedPassengerDtlsbyUserID(int UserID);
        Task<CommonRsult> SaveOrUpdatePassengerDetails(SavedPassengerDtl model);


    }
}
