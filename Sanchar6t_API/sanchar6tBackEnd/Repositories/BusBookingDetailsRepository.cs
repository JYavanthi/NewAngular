using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using Microsoft.AspNetCore.Http.HttpResults;

namespace sanchar6tBackEnd.Repositories
{
    public class BusBookingDetailsRepository : IBusBookingDetails
    { 
        private readonly Sanchar6tDbContext _context;

        public BusBookingDetailsRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async  Task<CommonRsult> GetBusBookingDtlByID(int BookingID)
        {
            CommonRsult result = new CommonRsult();
            try 
            {
                var data = await _context.VwBusBookingDetails.Where(m => m.BusBooKingDetailId == BookingID).ToListAsync();
                result.Type = "S";
                result.Count = data.Count();
                result.Data = data;
                result.Message = "Succesfully";
            }
            catch(Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }

        public async Task<CommonRsult> GetBusBookingDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBusBookingDetails.ToListAsync();
                result.Type = "S";
                result.Count = data.Count();
                result.Data = data;
                result.Message = "Succesfully";
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
                result.Type = "E";
            }
            return result;
        }

        public async Task<CommonRsult> SaveBusBookingdetails(EBusBookingDetails bookingDetails)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BusBookingDetails", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", bookingDetails.Flag);
                    cmd.Parameters.AddWithValue("@BusBooKingDetailID", bookingDetails.BusBooKingDetailID);
                    cmd.Parameters.AddWithValue("@UserID", bookingDetails.UserID);
                    cmd.Parameters.AddWithValue("@FromDate", bookingDetails.FromDate);
                    cmd.Parameters.AddWithValue("@ToDate", bookingDetails.ToDate);
                    cmd.Parameters.AddWithValue("@OperatorID", bookingDetails.OperatorID);
                    cmd.Parameters.AddWithValue("@AgentID", bookingDetails.AgentID);
                    cmd.Parameters.AddWithValue("@BoardingPoint", bookingDetails.BoardingPoint);
                    cmd.Parameters.AddWithValue("@DroppingPoint", bookingDetails.DroppingPoint);
                    cmd.Parameters.AddWithValue("@ScheduleID", bookingDetails.ScheduleID);
                    cmd.Parameters.AddWithValue("@DepartureTime", bookingDetails.DepartureTime);
                    cmd.Parameters.AddWithValue("@ArrivalTime", bookingDetails.ArrivalTime);
                    cmd.Parameters.AddWithValue("@BusNum", bookingDetails.BusNum);
                    cmd.Parameters.AddWithValue("@Status", bookingDetails.Status);
                    cmd.Parameters.AddWithValue("@SeatPrice", bookingDetails.SeatPrice);
                    cmd.Parameters.AddWithValue("@CreatedBy", bookingDetails.CreatedBy);


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
