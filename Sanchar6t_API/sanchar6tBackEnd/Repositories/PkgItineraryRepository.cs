using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgItineraryRepository : IPkgItinerary
    {
        private readonly Sanchar6tDbContext _context;

        public PkgItineraryRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetPkgItinerary()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgItineraries.ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetPkgItineraryByID(int PkgItineraryID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgItineraries.Where(m => m.PkgItineraryId == PkgItineraryID).ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetPkgItineraryByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgItineraries.Where(m => m.PackageId == PackageID).ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }


        public async Task<CommonRsult> SavePkgItinerary(EPkgItinerary pkgItinerary)
        {
            CommonRsult result = new CommonRsult();
            try
            {                    
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgItinerary", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkgItinerary.Flag);
                    cmd.Parameters.AddWithValue("@PkgItineraryID", pkgItinerary.PkgItineraryID);
                    cmd.Parameters.AddWithValue("@PackageID", pkgItinerary.PackageID);
                    cmd.Parameters.AddWithValue("@Day", pkgItinerary.Day);
                    cmd.Parameters.AddWithValue("@FromTime ", pkgItinerary.FromTime);
                    cmd.Parameters.AddWithValue("@ToTime ", pkgItinerary.ToTime);
                    cmd.Parameters.AddWithValue("@Title ", pkgItinerary.Title);
                    cmd.Parameters.AddWithValue("@Description", pkgItinerary.Description);
                    cmd.Parameters.AddWithValue("@IsActive", pkgItinerary.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkgItinerary.CreatedBy);


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