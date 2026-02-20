using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using sanchar6tBackEnd.Data;

namespace sanchar6tBackEnd.Repositories
{
    public class WalletTransactionRepository : IWalletTransaction
    {
        private readonly Sanchar6tDbContext _context;

        public WalletTransactionRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetWalletTransaction()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwWalletTransactions.ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetWalletTransactionByID(int userID)
        {

            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwWalletTransactions.Where(m => m.UserId == userID).ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }
        public async Task<CommonRsult> SaveWalletTransaction(EWalletTransaction walletTransaction)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_WalletTransaction", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", walletTransaction.Flag);
                    cmd.Parameters.AddWithValue("@WalletTrnsnID", walletTransaction.WalletTrnsnID);
                    cmd.Parameters.AddWithValue("@UserID", walletTransaction.UserID);
                    cmd.Parameters.AddWithValue("@Amount", walletTransaction.Amount);
                    cmd.Parameters.AddWithValue("@Date", walletTransaction.Date);
                    cmd.Parameters.AddWithValue("@Mode", walletTransaction.Mode);
                    cmd.Parameters.AddWithValue("@TransactionNumber", walletTransaction.TransactionNumber);
                    cmd.Parameters.AddWithValue("@ErrorCode", walletTransaction.ErrorCode); 
                    cmd.Parameters.AddWithValue("@TransactionCode", walletTransaction.TransactionCode);
                    cmd.Parameters.AddWithValue("@Message", walletTransaction.Message);
                    cmd.Parameters.AddWithValue("@CreatedBy", walletTransaction.CreatedBy);


                    using (var da = new SqlDataAdapter(cmd))
                    {
                        await Task.Run(() => da.Fill(dt));
                        result.Type = "S";
                        result.Message = "Insert Successfully";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }
    }
}
