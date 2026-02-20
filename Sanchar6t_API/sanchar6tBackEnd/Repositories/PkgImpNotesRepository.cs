using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgImpNotesRepository : IPkgImpNotes
    {
        private readonly Sanchar6tDbContext _context;

        public PkgImpNotesRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }


        public async Task<CommonRsult> SavePkgImpNotes(EPkgImpNotes pkgImpNotes)
        {
            CommonRsult result = new CommonRsult();
            try        //exception handling
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgImpNotes", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkgImpNotes.Flag);
                    cmd.Parameters.AddWithValue("@PkgImpNoteID", pkgImpNotes.PkgImpNoteID);
                    cmd.Parameters.AddWithValue("@PackageID", pkgImpNotes.PackageID);
                    cmd.Parameters.AddWithValue("@Description", pkgImpNotes.Description);
                    cmd.Parameters.AddWithValue("@IsActive", pkgImpNotes.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkgImpNotes.CreatedBy);


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

        public async Task<CommonRsult> GetpkgImpNotes()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImpNotes.ToListAsync();
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

        public async Task<CommonRsult> GetpkgImpNotesByID(int PkgImpNotesID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImpNotes.Where(m => m.PkgImpNoteId == PkgImpNotesID).ToListAsync();
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

        public async Task<CommonRsult> GetpkgImpNotesByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgImpNotes.Where(m => m.PackageId == PackageID).ToListAsync();
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
    }
}


