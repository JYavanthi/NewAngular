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
                var data = await _context.VwBusBookingSeats.Where(m =>m.BusBookingSeatId == BookingseatID).ToListAsync();
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
            catch(Exception ex)
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
                    cmd.Parameters.AddWithValue("@UserID", busBookingSeat.UserID);
                    cmd.Parameters.AddWithValue("@ForSelf", busBookingSeat.ForSelf);
                    cmd.Parameters.AddWithValue("@IsPrimary", busBookingSeat.IsPrimary);
                    cmd.Parameters.AddWithValue("@SeatNo", busBookingSeat.SeatNo);
                    cmd.Parameters.AddWithValue("@FirstName", busBookingSeat.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", busBookingSeat.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", busBookingSeat.LastName);
                    cmd.Parameters.AddWithValue("@Email", busBookingSeat.Email);
                    cmd.Parameters.AddWithValue("@ContactNo", busBookingSeat.ContactNo);
                    cmd.Parameters.AddWithValue("@Gender", busBookingSeat.Gender);
                    cmd.Parameters.AddWithValue("@AadharNo", busBookingSeat.AadharNo);
                    cmd.Parameters.AddWithValue("@PancardNo", busBookingSeat.PancardNo);
                    cmd.Parameters.AddWithValue("@BloodGroup", busBookingSeat.BloodGroup);
                    cmd.Parameters.AddWithValue("@DOB", busBookingSeat.DOB);
                    cmd.Parameters.AddWithValue("@FoodPref", busBookingSeat.FoodPref);
                    cmd.Parameters.AddWithValue("@CreatedBy", busBookingSeat.CreatedBy);

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
