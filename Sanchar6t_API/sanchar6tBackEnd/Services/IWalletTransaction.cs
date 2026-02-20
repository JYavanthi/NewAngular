using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IWalletTransaction
    {
        Task<CommonRsult> SaveWalletTransaction(EWalletTransaction walletTransaction);
        Task<CommonRsult> GetWalletTransaction();
        Task<CommonRsult> GetWalletTransactionByID(int userID);
    }
}
