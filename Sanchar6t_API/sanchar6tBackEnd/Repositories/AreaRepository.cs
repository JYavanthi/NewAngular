using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class AreaRepository : IArea
    {
        private readonly Sanchar6tDbContext _context;

        public AreaRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetArea()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwAreas.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveArea(EArea eArea)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Area", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eArea.Flag);
                    cmd.Parameters.AddWithValue("@AreaID", eArea.AreaID);
                    cmd.Parameters.AddWithValue("@AreaName", eArea.AreaName);
                    cmd.Parameters.AddWithValue("@Pincode", eArea.Pincode);
                    cmd.Parameters.AddWithValue("@Longitude", eArea.Longitude);
                    cmd.Parameters.AddWithValue("@Latitude", eArea.Latitude);
                    //cmd.Parameters.AddWithValue("@CityID", eArea.CityID);
                    cmd.Parameters.AddWithValue("@CreatedBy", eArea.CreatedBy);


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
