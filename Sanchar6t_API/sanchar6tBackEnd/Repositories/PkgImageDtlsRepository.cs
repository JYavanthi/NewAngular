using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgImageDtlsRepository : IPkgImageDtls
    {
        private readonly Sanchar6tDbContext _context;

        public PkgImageDtlsRepository(Sanchar6tDbContext context) 
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetPkgImageDtlByID(int PkgImageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImageDtls.Where(m =>m.PackageId == PkgImageID).ToListAsync();
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

        public async Task<CommonRsult> GetPkgImageDtlByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImageDtls.Where(m =>m.PackageId == PackageID).ToListAsync();
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

        public async Task<CommonRsult> GetPkgImageDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImageDtls.ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
                result.Data = data;
                result.Count = data.Count();
            }
            catch(Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SavePkgImageDtls(EPkgImageDtls pkgimagedtls)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgImageDtls", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkgimagedtls.Flag);
                    cmd.Parameters.AddWithValue("@PkgImageID", pkgimagedtls.PkgImageID);
                    cmd.Parameters.AddWithValue("@PackageID", pkgimagedtls.PackageID);
                    cmd.Parameters.AddWithValue("@PkgImage", pkgimagedtls.PkgImage);
                    cmd.Parameters.AddWithValue("@PkgSection ", pkgimagedtls.PkgSection);
                    cmd.Parameters.AddWithValue("@Heading ", pkgimagedtls.Heading);
                    cmd.Parameters.AddWithValue("@SubHeading ", pkgimagedtls.SubHeading);
                    cmd.Parameters.AddWithValue("@BtnName ", pkgimagedtls.BtnName);
                    cmd.Parameters.AddWithValue("@BtnUrl ", pkgimagedtls.BtnUrl);
                    cmd.Parameters.AddWithValue("@IsActive", pkgimagedtls.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkgimagedtls.CreatedBy);


                    using (var da = new SqlDataAdapter(cmd))
                    {
                        await Task.Run(() => da.Fill(dt));
                        result.Type = "S";
                        result.Message = "Insert Successfully";
                        if (pkgimagedtls.Flag == "I" && dt.Rows.Count > 0)
                        {
                            result.Data = dt.Rows[0]["NewPkgImageID"];  
                        }
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