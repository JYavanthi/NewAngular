using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IWallet
    {
        Task<CommonRsult> SaveWallet(EWallet wallet);
        Task<CommonRsult> GetWallet(int UserID);
        Task<CommonRsult> GetWalletByID(int WalletTransactionID);
    }
}
