using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Identity.Client;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using System.Security.AccessControl;

namespace sanchar6tBackEnd.Repositories
{
    public class PackageRepository : IPackage
    {
        private readonly Sanchar6tDbContext _context;

        public PackageRepository(Sanchar6tDbContext context)
        { 
            this._context = context;
        }

        public async Task<CommonRsult> GetPackageDtlByID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPackages.Where(m =>m.PackageId == PackageID).ToListAsync();
                result.Data = data;
                result.Type = "S";
                result.Message = "Sucessfully";
                result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }

        public async Task<CommonRsult> GetPackageDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPackages.ToListAsync();
                result.Data = data;
                result.Type = "S";
                result.Message = "Sucessfully";
                result.Count = data.Count();
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }

        public async Task<CommonRsult> SavePackagedetails(EPackage package)
        {
            CommonRsult result = new CommonRsult();
            try
            {                 
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Package", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", package.Flag);
                    SqlParameter outputPackageID = new SqlParameter("@PackageID", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputPackageID);
                    cmd.Parameters.AddWithValue("@PackageName", package.PackageName);
                    cmd.Parameters.AddWithValue("@State", package.State);
                    cmd.Parameters.AddWithValue("@Country", package.Country);
                    cmd.Parameters.AddWithValue("@From", package.From);
                    cmd.Parameters.AddWithValue("@To", package.To);
                    cmd.Parameters.AddWithValue("@Noofdays", package.Noofdays);
                    cmd.Parameters.AddWithValue("@Shortdescription", package.Shortdescription);
                    cmd.Parameters.AddWithValue("@Description", package.Description);
                    cmd.Parameters.AddWithValue("@AdditionalNotes", package.AdditionalNotes);
                    cmd.Parameters.AddWithValue("@PackagePrice", package.PackagePrice);
                    cmd.Parameters.AddWithValue("@CreatedBy", package.CreatedBy);


                    con.Open();
                    await cmd.ExecuteNonQueryAsync();
                    con.Close();

                    int insertedPackageID = (int)cmd.Parameters["@PackageID"].Value;

                    result.Type = "S";
                    result.Message = "Insert Successfully";
                    result.Data = new { PackageID = insertedPackageID };
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
