using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPayment
    {
        Task<CommonRsult> SavePayment(EPayment ePayment);
        Task<CommonRsult> GetPayment();

    }
}
