using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgIncludeRepository : IPkgInclude
    {
        private readonly Sanchar6tDbContext _context;

        public PkgIncludeRepository(Sanchar6tDbContext context)
        { 
            this._context = context;
        }

        public async Task<CommonRsult> GetPkgInclude()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgIncludes.ToListAsync();
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

        public async Task<CommonRsult> GetPkgIncludeByID(int PkgIncludeID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgIncludes.Where(m => m.PkgIncludeId == PkgIncludeID).ToListAsync();
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

        public async Task<CommonRsult> GetPkgIncludeByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgIncludes.Where(m => m.PackageId == PackageID).ToListAsync();
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


        public async Task<CommonRsult> SavePkgInclude(EPkgInclude pkgInclude)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgInclude", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkgInclude.Flag);
                    cmd.Parameters.AddWithValue("@PkgIncludeID", pkgInclude.PkgIncludeID);
                    cmd.Parameters.AddWithValue("@PackageID", pkgInclude.PackageID);
                    cmd.Parameters.AddWithValue("@Description", pkgInclude.Description);
                    cmd.Parameters.AddWithValue("@IsIncluded", pkgInclude.IsIncluded);
                    cmd.Parameters.AddWithValue("@IsActive", pkgInclude.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkgInclude.CreatedBy);


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
