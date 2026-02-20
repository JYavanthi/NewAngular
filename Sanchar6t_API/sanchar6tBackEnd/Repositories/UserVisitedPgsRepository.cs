using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class UserVisitedPgsRepository : IUserVisitedPgs
    {
        private readonly Sanchar6tDbContext context;

        public UserVisitedPgsRepository(Sanchar6tDbContext context)
        {
            this.context = context;
        }

        public async Task<CommonRsult> SaveIUserVisitedPgs(EUserVisitedPgs userVisitedPgs)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_UserVisitedPgs", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", userVisitedPgs.Flag);
                    cmd.Parameters.AddWithValue("@UserVisitedPgID", userVisitedPgs.UserVisitedPgID);
                    cmd.Parameters.AddWithValue("@UserID", userVisitedPgs.UserID);
                    cmd.Parameters.AddWithValue("@VisitedPgTimeFrom", userVisitedPgs.VisitedPgTimeFrom);
                    cmd.Parameters.AddWithValue("@VisitedPgTimeTo", userVisitedPgs.VisitedPgTimeTo);
                    cmd.Parameters.AddWithValue("@CreatedBy", userVisitedPgs.CreatedBy); using (var da = new SqlDataAdapter(cmd))
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

        public Task<CommonRsult> SaveUserVisitedPgs(EUserVisitedPgs userVisitedPgs)
        {
            throw new NotImplementedException();
        }
    }
}
