using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using System.Collections.Specialized;
using System.Data;

namespace sanchar6tBackEnd.Repositories
{
    public class UserRepository : IUser
    {
        private readonly Sanchar6tDbContext _context;

        public UserRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> SaveUser(User user)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_User", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", user.Flag);
                    cmd.Parameters.AddWithValue("@UserID", user.UserID);
                    cmd.Parameters.AddWithValue("@UserType", user.UserType);
                    cmd.Parameters.AddWithValue("@Status", user.Status);
                    cmd.Parameters.AddWithValue("@Password", user.Password);
                    cmd.Parameters.AddWithValue("@FirstName", user.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", user.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", user.LastName);
                    cmd.Parameters.AddWithValue("@Email", user.Email);
                    cmd.Parameters.AddWithValue("@ContactNo", user.ContactNo);
                    cmd.Parameters.AddWithValue("@Gender", user.Gender);
                    cmd.Parameters.AddWithValue("@AadharNo", user.AadharNo);
                    cmd.Parameters.AddWithValue("@PancardNo", user.PancardNo);
                    cmd.Parameters.AddWithValue("@BloodGroup", user.BloodGroup);
                    cmd.Parameters.AddWithValue("@PrimaryUser", user.PrimaryUser);
                    cmd.Parameters.AddWithValue("@Age", user.Age);
                    cmd.Parameters.AddWithValue("@Address", user.Address);
                    cmd.Parameters.AddWithValue("@AlternativeNumber", user.AlternativeNumber);
                    cmd.Parameters.AddWithValue("@Remarks", user.Remarks);
                    cmd.Parameters.AddWithValue("@CompanyName", user.CompanyName);
                    cmd.Parameters.AddWithValue("@CompanyID", user.CompanyID);
                    cmd.Parameters.AddWithValue("@CompanyAddress", user.CompanyAddress);
                    cmd.Parameters.AddWithValue("@ShopAddress", user.ShopAddress);
                    cmd.Parameters.AddWithValue("@Organisation ", user.Organisation);
                    cmd.Parameters.AddWithValue("@City ", user.City);
                    cmd.Parameters.AddWithValue("@State ", user.State);
                    cmd.Parameters.AddWithValue("@Comments ", user.Comments);
                    cmd.Parameters.AddWithValue("@GST", user.GST);
                    cmd.Parameters.AddWithValue("@Amount", user.Amount);
                    cmd.Parameters.AddWithValue("@Type", user.Type);
                    cmd.Parameters.AddWithValue("@TransactionLimit", user.TransactionLimit);
                    cmd.Parameters.AddWithValue("@CreatedBy", user.CreatedBy);
                    

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


        public async Task<CommonRsult> GetUser()
        {

            CommonRsult result = new CommonRsult();

            try
            {
                var data = await _context.VwUsers.ToListAsync();
                result.Data = data;
                result.Count = data.Count();
                result.Message = "Successfully";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            return result;
        }

        public async Task<CommonRsult> GetUserByID(int userID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwUsers.Where(m => m.UserId == userID).ToListAsync();
                result.Data = data;
                result.Count = data.Count();
                result.Message = "Successfully";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                return result;
            }
            return result;
        }
    }
}
