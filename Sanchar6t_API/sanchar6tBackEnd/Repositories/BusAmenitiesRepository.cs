using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class BusAmenitiesRepository : IBusAmenities
    {
        private readonly Sanchar6tDbContext _context;

        public BusAmenitiesRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetBusAmenities()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBusAmenities.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveBusAmenities(EBusAmenities eBusAmenities)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BusAmenities", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eBusAmenities.Flag);
                    cmd.Parameters.AddWithValue("@BusAmenitiesID", eBusAmenities.BusAmenitiesID);
                    cmd.Parameters.AddWithValue("@BusOperatorID", eBusAmenities.BusOperatorID);
                    cmd.Parameters.AddWithValue("@AmenityID", eBusAmenities.AmenityID);
                    cmd.Parameters.AddWithValue("@CreatedBy", eBusAmenities.CreatedBy);


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
