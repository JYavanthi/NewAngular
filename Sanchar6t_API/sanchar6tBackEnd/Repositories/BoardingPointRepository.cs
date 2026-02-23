using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class BoardingPointRepository : IBoardingPoint
    {
        private readonly Sanchar6tDbContext _context;

        public BoardingPointRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }


        public async Task<CommonRsult> GetBoardingPoint()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBoardingPoints.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveBoardingPoint(EBoardingPoint eBoardingPoint)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BoardingPoint", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eBoardingPoint.Flag);
                    cmd.Parameters.AddWithValue("@BoardingPointID", eBoardingPoint.BoardingPointID);
                    cmd.Parameters.AddWithValue("@id", eBoardingPoint.id);
                    cmd.Parameters.AddWithValue("@AreaID", eBoardingPoint.AreaID);
                    cmd.Parameters.AddWithValue("@Landmark", eBoardingPoint.Landmark);
                    cmd.Parameters.AddWithValue("@Town", eBoardingPoint.Town);
                    cmd.Parameters.AddWithValue("@latitude", eBoardingPoint.latitude);
                    cmd.Parameters.AddWithValue("@longitude", eBoardingPoint.longitude);
                    cmd.Parameters.AddWithValue("@CreatedBy", eBoardingPoint.CreatedBy);


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
