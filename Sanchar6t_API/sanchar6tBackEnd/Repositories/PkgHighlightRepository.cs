using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgHighlightRepository : IPkgHighlight
    {
        private readonly Sanchar6tDbContext _context;

        public PkgHighlightRepository(Sanchar6tDbContext context)
        { 
            this._context = context;
        }

        public async Task<CommonRsult> GetPkgHighlight()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgHighlights.ToListAsync();
                result.Count = data.Count();
                result.Type = "S";
                result.Message = "Sucessfully";
                result.Data = data;
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }

        public async Task<CommonRsult> GetPkgHighlightByID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgHighlights.Where(m => m.PackageId == PackageID).ToListAsync();
                result.Count = data.Count();
                result.Type = "S";
                result.Message = "Sucessfully";
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }
        public async Task<CommonRsult> SavePkgHighlight(EPkgHighlight pkghighlight)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgHighlight", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkghighlight.Flag);
                    cmd.Parameters.AddWithValue("@PkgHighlightID", pkghighlight.PkgHighlightID);
                    cmd.Parameters.AddWithValue("@PackageID", pkghighlight.PackageID);
                    cmd.Parameters.AddWithValue("@Title", pkghighlight.Title);
                    cmd.Parameters.AddWithValue("@Description", pkghighlight.Description);
                    cmd.Parameters.AddWithValue("@IsActive", pkghighlight.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkghighlight.CreatedBy);


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