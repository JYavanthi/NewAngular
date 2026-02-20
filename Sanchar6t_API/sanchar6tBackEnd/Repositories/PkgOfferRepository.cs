using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class PkgOfferRepository : IPkgOffer
    {
        private readonly Sanchar6tDbContext _context;

        public PkgOfferRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetpkgOffer()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgOffers.ToListAsync();
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

        public async Task<CommonRsult> GetpkgOfferByID(int pkgOfferID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgOffers.Where(m => m.PkgOfferId == pkgOfferID).ToListAsync();
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

        public async Task<CommonRsult> GetpkgOfferByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwPkgOffers.Where(m => m.PackageId == PackageID).ToListAsync();
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



        public async Task<CommonRsult> SavepkgOffer(EPkgOffer pkgOffer)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_PkgOffer", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", pkgOffer.Flag);
                    cmd.Parameters.AddWithValue("@PkgOfferID ", pkgOffer.PkgOfferID);
                    cmd.Parameters.AddWithValue("@PackageID", pkgOffer.PackageID);
                    cmd.Parameters.AddWithValue("@Price ", pkgOffer.Price);
                    cmd.Parameters.AddWithValue("@OfferPrice ", pkgOffer.OfferPrice);
                    cmd.Parameters.AddWithValue("@EffectiveDate  ", pkgOffer.EffectiveDate);
                    cmd.Parameters.AddWithValue("@WeekDay  ", pkgOffer.WeekDay);
                    cmd.Parameters.AddWithValue("@IsActive", pkgOffer.IsActive);
                    cmd.Parameters.AddWithValue("@CreatedBy", pkgOffer.CreatedBy);


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