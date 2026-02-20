using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class countriesRepository : Icountries
    {
        private readonly Sanchar6tDbContext _context;

        public countriesRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }



        public async Task<CommonRsult> GetCountries()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                //var data = await _context.VwCountries.ToListAsync();
                //result.Data = data.OrderByDescending(m => m.Id);
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetCountryByID(int countryID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                //var data = await _context.VwCountries.Where(m => m.Id == countryID).ToListAsync();
                //result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> Savecountries(Ecountries ecountries)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_countries", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", ecountries.Flag);
                    cmd.Parameters.AddWithValue("@id", ecountries.id);
                    cmd.Parameters.AddWithValue("@name", ecountries.name);
                    cmd.Parameters.AddWithValue("@iso3", ecountries.iso3);
                    cmd.Parameters.AddWithValue("@iso2", ecountries.iso2);
                    cmd.Parameters.AddWithValue("@numeric_code", ecountries.numeric_code);
                    cmd.Parameters.AddWithValue("@phonecode", ecountries.phonecode);
                    cmd.Parameters.AddWithValue("@capital", ecountries.capital);
                    cmd.Parameters.AddWithValue("@currency", ecountries.currency);
                    cmd.Parameters.AddWithValue("@currency_name", ecountries.currency_name);
                    cmd.Parameters.AddWithValue("@currency_symbol", ecountries.currency_symbol);
                    cmd.Parameters.AddWithValue("@tld", ecountries.tld);
                    cmd.Parameters.AddWithValue("@native", ecountries.native);
                    cmd.Parameters.AddWithValue("@region", ecountries.region);
                    cmd.Parameters.AddWithValue("@region_id", ecountries.region_id);
                    cmd.Parameters.AddWithValue("@subregion", ecountries.subregion);
                    cmd.Parameters.AddWithValue("@subregion_id", ecountries.subregion_id);
                    cmd.Parameters.AddWithValue("@nationality", ecountries.nationality);
                    cmd.Parameters.AddWithValue("@timezones", ecountries.timezones);
                    cmd.Parameters.AddWithValue("@latitude", ecountries.latitude);
                    cmd.Parameters.AddWithValue("@longitude", ecountries.longitude);
                    cmd.Parameters.AddWithValue("@emoji", ecountries.emoji);
                    cmd.Parameters.AddWithValue("@emojiU", ecountries.emojiU);
                    cmd.Parameters.AddWithValue("@CreatedBy", ecountries.CreatedBy);


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


