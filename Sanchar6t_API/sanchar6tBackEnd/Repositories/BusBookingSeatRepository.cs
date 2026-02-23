using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class BusBookingSeatRepository : IBusBookingSeat
    {
        private readonly Sanchar6tDbContext _context;

        public BusBookingSeatRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }
        public async Task<CommonRsult> GetBookingSeatDtlByID(int BookingseatID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBusBookingSeats.Where(m => m.BusBookingSeatId == BookingseatID).ToListAsync();
                result.Type = "S";
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

        public async Task<CommonRsult> GetBookingSeatDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwBusBookingSeats.ToListAsync();
                result.Type = "S";
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

        public async Task<CommonRsult> SaveBookingSeatdetails(EBusBookingSeat busBookingSeat)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_BusBookingSeat", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", busBookingSeat.Flag);
                    cmd.Parameters.AddWithValue("@BusBookingSeatID", busBookingSeat.BusBookingSeatID);
                    cmd.Parameters.AddWithValue("@BusBookingDetailsId", busBookingSeat.BusBookingDetailsId);
                    cmd.Parameters.AddWithValue("@UserID", busBookingSeat.UserID);
                    cmd.Parameters.AddWithValue("@ForSelf", busBookingSeat.ForSelf);
                    cmd.Parameters.AddWithValue("@IsPrimary", busBookingSeat.IsPrimary);
                    cmd.Parameters.AddWithValue("@SeatNo", busBookingSeat.SeatNo);
                    cmd.Parameters.AddWithValue("@FirstName", busBookingSeat.FirstName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@MiddleName", busBookingSeat.MiddleName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LastName", busBookingSeat.LastName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Email", busBookingSeat.Email ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@ContactNo", busBookingSeat.ContactNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Gender", busBookingSeat.Gender ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@AadharNo", busBookingSeat.AadharNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PancardNo", busBookingSeat.PancardNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BloodGroup", busBookingSeat.BloodGroup ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DOB", busBookingSeat.DOB ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@FoodPref", busBookingSeat.FoodPref ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@BusOperatorId", busBookingSeat.BusOperatorId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Disabled", busBookingSeat.Disabled ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Pregnant", busBookingSeat.Pregnant ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegisteredCompanyNumber", busBookingSeat.RegisteredCompanyNumber ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RegisteredCompanyName", busBookingSeat.RegisteredCompanyName ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DrivingLicence", busBookingSeat.DrivingLicence ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PassportNo", busBookingSeat.PassportNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@RationCard", busBookingSeat.RationCard ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@VoterId", busBookingSeat.VoterId ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Others", busBookingSeat.Others ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Nri", busBookingSeat.Nri ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@SavePassengerDetails", busBookingSeat.SavePassengerDetails ?? "No");
                    cmd.Parameters.AddWithValue("@Status", busBookingSeat.Status ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@LockStatus", busBookingSeat.LockStatus ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CancelledBy", busBookingSeat.CancelledBy ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CancelledDate", busBookingSeat.CancelledDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@PaymentStatus", busBookingSeat.PaymentStatus ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@JourneyDate", busBookingSeat.JourneyDate ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@TicketNo", busBookingSeat.TicketNo ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedBy", busBookingSeat.UserID);



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


