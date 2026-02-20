using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
        public class BookItineraryRepository : IBookItinerary
        {

        private readonly  Sanchar6tDbContext _context;
          
        public BookItineraryRepository(Sanchar6tDbContext context)
            {
            this._context = context;
        }

            public async Task<CommonRsult> BookItinerarydetails(EBookItinerary bookItinerary)
            {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BookItinerary", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", bookItinerary.Flag);
                    cmd.Parameters.AddWithValue("@ItineraryID", bookItinerary.ItineraryID);
                    cmd.Parameters.AddWithValue("@PackageID", bookItinerary.PackageID);
                    cmd.Parameters.AddWithValue("@Image", bookItinerary.Image);
                    cmd.Parameters.AddWithValue("@Day", bookItinerary.Day);
                    cmd.Parameters.AddWithValue("@Title", bookItinerary.Title);
                    cmd.Parameters.AddWithValue("@Description", bookItinerary.Description);
                    cmd.Parameters.AddWithValue("@CreatedBy", bookItinerary.CreatedBy);

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
