using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class UsertypeRepository : IUsertype
    {
        private readonly Sanchar6tDbContext _context;

        public UsertypeRepository(Sanchar6tDbContext context)
        { 
            this._context = context;
        }

        public async Task<CommonRsult> GetUsertype()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                //var data = await _context.VwUsertypes.ToListAsync();
                //result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveUsertype(EUsertype eUsertype)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Usertype", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eUsertype.Flag);
                    cmd.Parameters.AddWithValue("@UsertypeId", eUsertype.UsertypeId);
                    cmd.Parameters.AddWithValue("@Usertype", eUsertype.Usertype);
                    cmd.Parameters.AddWithValue("@CreatedBy", eUsertype.CreatedBy);
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
