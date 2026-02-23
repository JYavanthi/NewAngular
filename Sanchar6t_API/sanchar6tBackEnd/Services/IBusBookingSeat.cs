using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBusBookingSeat
    {
        Task<CommonRsult> SaveBookingSeatdetails(EBusBookingSeat busBookingSeat);
        Task<CommonRsult> GetBookingSeatDtls();
        Task<CommonRsult> GetBookingSeatDtlByID(int BookingseatID);
    }
}
 