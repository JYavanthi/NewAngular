using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBusOperator
    {
        Task<CommonRsult> SaveBusOperator(EBusOperator eBusOperator);
        Task<CommonRsult> GetBusOperator();
    }
}
