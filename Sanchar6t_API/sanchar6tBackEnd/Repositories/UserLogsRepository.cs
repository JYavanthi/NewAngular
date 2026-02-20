using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Repositories
{
    public class UserLogsRepository : IUserLogs
    {
        private readonly Sanchar6tDbContext _context;

        public UserLogsRepository(Sanchar6tDbContext context)
        {
            this._context = context; 
        }

        public async  Task<CommonRsult> GetUserLogs()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                //var data = await _context.VwUserLogs.ToListAsync();
                //result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveUserLogs(EUserLogs userLogs)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_UserLogs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", userLogs.Flag);
                    cmd.Parameters.AddWithValue("@UserlogID", userLogs.UserlogID);
                    cmd.Parameters.AddWithValue("@UserID", userLogs.UserID);
                    cmd.Parameters.AddWithValue("@LoginTime", userLogs.LoginTime);
                    cmd.Parameters.AddWithValue("@LogoutTime", userLogs.LogoutTime);
                    cmd.Parameters.AddWithValue("@Token", userLogs.Token);
                    cmd.Parameters.AddWithValue("@CreatedBy", userLogs.CreatedBy);
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

