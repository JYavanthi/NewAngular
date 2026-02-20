using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class UserSearchRepository : IUserSearch
    {
        private readonly Sanchar6tDbContext _context;

        public UserSearchRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }
        public async Task<CommonRsult> GetUserSearch()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwUserSearches.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;

        }

        public async Task<CommonRsult> SaveUserSearch(EUserSearch eUserSearch)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_UserSearch", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eUserSearch.Flag);
                    cmd.Parameters.AddWithValue("@UserID ", eUserSearch.UserID);
                    cmd.Parameters.AddWithValue("@SearchedDate", eUserSearch.SearchedDate);
                    cmd.Parameters.AddWithValue("@From ", eUserSearch.From);
                    cmd.Parameters.AddWithValue("@To ", eUserSearch.To);
                    cmd.Parameters.AddWithValue("@ModeOfTransport  ", eUserSearch.ModeOfTransport);
                    cmd.Parameters.AddWithValue("@Operator  ", eUserSearch.Operator);
                    cmd.Parameters.AddWithValue("@CreatedBy", eUserSearch.CreatedBy);


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
