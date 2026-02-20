using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class BusOperatorRepository : IBusOperator
    {
        private readonly Sanchar6tDbContext _context;

        public BusOperatorRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetBusOperator()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBusOperators.ToListAsync();
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveBusOperator(EBusOperator eBusOperator)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BusOperator", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eBusOperator.Flag);
                    cmd.Parameters.AddWithValue("@BusOperatorID", eBusOperator.BusOperatorID);
                    cmd.Parameters.AddWithValue("@BusNo", eBusOperator.BusNo);
                    cmd.Parameters.AddWithValue("@BusType", eBusOperator.BusType);
                    cmd.Parameters.AddWithValue("@BusSeats", eBusOperator.BusSeats);
                    cmd.Parameters.AddWithValue("@FemaleSeatNo", eBusOperator.FemaleSeatNo);
                    cmd.Parameters.AddWithValue("@MaleSeatNo", eBusOperator.MaleSeatNo);
                    cmd.Parameters.AddWithValue("@CreatedBy", eBusOperator.CreatedBy);


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
