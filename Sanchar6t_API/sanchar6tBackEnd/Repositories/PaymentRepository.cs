using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PaymentRepository : IPayment
    {
        private readonly Sanchar6tDbContext _context;

        public PaymentRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetPayment()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPayments.ToListAsync();
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

        public async Task<CommonRsult> SavePayment(EPayment ePayment)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Payment", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", ePayment.Flag);
                    cmd.Parameters.AddWithValue("@PaymentID", ePayment.PaymentID);
                    cmd.Parameters.AddWithValue("@UserID", ePayment.UserID);
                    cmd.Parameters.AddWithValue("@BookingdtlsID", ePayment.BookingdtlsID);
                    cmd.Parameters.AddWithValue("@Amount", ePayment.Amount);
                    cmd.Parameters.AddWithValue("@PaymentMode", ePayment.PaymentMode);
                    cmd.Parameters.AddWithValue("@TransactionID", ePayment.TransactionID);
                    cmd.Parameters.AddWithValue("@TransactionResponse", ePayment.TransactionResponse);
                    cmd.Parameters.AddWithValue("@TransactionCode", ePayment.TransactionCode);
                    cmd.Parameters.AddWithValue("@PaymentStatus", ePayment.PaymentStatus);
                    cmd.Parameters.AddWithValue("@ErrorCode", ePayment.ErrorCode);
                    cmd.Parameters.AddWithValue("@CreatedBy", ePayment.CreatedBy);


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
