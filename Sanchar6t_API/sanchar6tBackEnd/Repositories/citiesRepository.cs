using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class citiesRepository : Icities
    {
        private readonly Sanchar6tDbContext _context;

        public citiesRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> Getcities()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                // Filter only cities with country_id = 101 (India)
                //var data = await _context.VwCities
                //    .Where(c => c.CountryId == 101)
                //    .OrderByDescending(c => c.Id)
                //    .ToListAsync();

                //result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }




        //public async Task<CommonRsult> Getcities(int pageIndex, int pageSize)
        //{
        //    CommonRsult result = new CommonRsult();
        //    try
        //    {
        //        var data = await _context.VwCities
        //            .Where(c => c.CountryId == 101)
        //            .OrderByDescending(c => c.Id)
        //            .Skip(pageIndex * pageSize)
        //            .Take(pageSize)
        //            .ToListAsync();

        //        result.Data = data;
        //    }
        //    catch (Exception ex)
        //    {
        //        result.Message = ex.Message;
        //    }
        //    return result;
        //}

        public async Task<CommonRsult> Savecities(Ecities ecities)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_cities", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", ecities.Flag);
                    cmd.Parameters.AddWithValue("@id", ecities.id);
                    cmd.Parameters.AddWithValue("@name", ecities.name);
                    cmd.Parameters.AddWithValue("@state_id", ecities.state_id);
                    cmd.Parameters.AddWithValue("@country_id", ecities.country_id);
                    cmd.Parameters.AddWithValue("@latitude", ecities.latitude);
                    cmd.Parameters.AddWithValue("@longitude", ecities.longitude);
                    cmd.Parameters.AddWithValue("@wikiDataId", ecities.wikiDataId);
                    cmd.Parameters.AddWithValue("@CreatedBy", ecities.CreatedBy);


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
