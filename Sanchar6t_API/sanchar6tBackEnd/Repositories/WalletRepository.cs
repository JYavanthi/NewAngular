using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class WalletRepository : IWallet
    {
        private readonly Sanchar6tDbContext _context;

        public WalletRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetWallet(int UserID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwWallets.Where(m => m.UserId == UserID).ToListAsync();
                result.Type = "S";
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

        public async Task<CommonRsult> GetWalletByID(int WalletID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwWallets.Where(m => m.WalletId == WalletID).ToListAsync();
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

        public async Task<CommonRsult> SaveWallet(EWallet wallet)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Wallet", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@WalletID", wallet.WalletID);
                    cmd.Parameters.AddWithValue("@UserID", wallet.UserID);
                    cmd.Parameters.AddWithValue("@Amount", wallet.Amount);
                    cmd.Parameters.AddWithValue("@Type", wallet.Type);
                    cmd.Parameters.AddWithValue("@TransactionLimit", wallet.TransactionLimit);
                    cmd.Parameters.AddWithValue("@CreatedBy", wallet.CreatedBy);

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

