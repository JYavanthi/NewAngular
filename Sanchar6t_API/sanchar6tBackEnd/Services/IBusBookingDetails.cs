using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBusBookingDetails
    {
        Task<CommonRsult> SaveBusBookingdetails(EBusBookingDetails bookingDetails);
        Task<CommonRsult> GetBusBookingDtls();
        Task<CommonRsult> GetBusBookingDtlByID(int BookingID);


    }
}
 