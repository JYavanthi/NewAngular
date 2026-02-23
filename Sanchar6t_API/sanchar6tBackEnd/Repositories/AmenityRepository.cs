using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class AmenityRepository : IAmenity
    {
        private readonly Sanchar6tDbContext _context;

        public AmenityRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetAmenity()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwAmenities.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveAmenity(EAmenity eAmenity)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Amenity", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eAmenity.Flag);
                    cmd.Parameters.AddWithValue("@AmenityID", eAmenity.AmenityID);
                    cmd.Parameters.AddWithValue("@AMIcon", eAmenity.AMIcon);
                    cmd.Parameters.AddWithValue("@AMName", eAmenity.AMName);
                    cmd.Parameters.AddWithValue("@CreatedBy", eAmenity.CreatedBy);


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
