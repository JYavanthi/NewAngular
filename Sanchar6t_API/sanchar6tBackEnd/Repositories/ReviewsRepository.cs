using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class ReviewsRepository : IReviews
    {
        private readonly Sanchar6tDbContext _context;

        public ReviewsRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetReviews()
        {

            CommonRsult result = new CommonRsult();
            try
            {
                //var data = await _context.VwReviews.ToListAsync();
                //result.Type = "S";
                //result.Data = data;
                //result.Count = data.Count();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveReviews(EReviews reviews)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_Reviews", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", reviews.Flag);
                    cmd.Parameters.AddWithValue("@ReviewID", reviews.ReviewID);
                    cmd.Parameters.AddWithValue("@BusBooKingDetailID", reviews.BusBooKingDetailID);
                    cmd.Parameters.AddWithValue("@UserID", reviews.UserID);
                    ////cmd.Parameters.AddWithValue("@DroppingPoint", reviews.DroppingPoint);
                    cmd.Parameters.AddWithValue("@Rating", reviews.Rating);
                    cmd.Parameters.AddWithValue("@Description", reviews.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", reviews.CreatedBy);

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
