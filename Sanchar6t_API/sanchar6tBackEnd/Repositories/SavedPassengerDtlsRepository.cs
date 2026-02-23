using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using Microsoft.Data.SqlClient;
using System.Threading.Tasks;

namespace sanchar6tBackEnd.Repositories
{
    public class SavedPassengerDtlsRepository : ISavedPassengerDtls
    {
        private readonly Sanchar6tDbContext _context;

        public SavedPassengerDtlsRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetSavedPassengerDtlsbyUserID(int UserID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.SavedPassengerDtls.FirstOrDefaultAsync(x => x.UserId == UserID);
                result.Data = data;
            }
            catch (Exception ex)
            {
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveOrUpdatePassengerDetails(SavedPassengerDtl model)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var sql = "EXEC sp_PassengerDtls " +
                          "@Flag = @Flag, @PassengerDtlID = @PassengerDtlID, @UserID = @UserID, " +
                          "@FirstName = @FirstName, @MiddleName = @MiddleName, @LastName = @LastName, " +
                          "@Email = @Email, @ContactNo = @ContactNo, @Gender = @Gender, " +
                          "@AadharNo = @AadharNo, @PancardNo = @PancardNo,@DrivingLicence= @DrivingLicence, @PassportNo = @PassportNo,@RationCard = @RationCard,@VoterId = @VoterId,@Others = @Others,@NRI = @NRI  ,@BloodGroup = @BloodGroup, " +
                          "@PrimaryUser = @PrimaryUser, @DOB = @DOB, @FoodPref = @FoodPref, @CreatedBy = @CreatedBy";

                var parameters = new[]
                {
            new SqlParameter("@Flag", "U"),
            new SqlParameter("@PassengerDtlID", model.PassengerDtlId),
            new SqlParameter("@UserID", model.UserId),
            new SqlParameter("@FirstName", model.FirstName ?? (object)DBNull.Value),
            new SqlParameter("@MiddleName", model.MiddleName ?? (object)DBNull.Value),
            new SqlParameter("@LastName", model.LastName ?? (object)DBNull.Value),
            new SqlParameter("@Email", model.Email ?? (object)DBNull.Value),
            new SqlParameter("@ContactNo", model.ContactNo ?? (object)DBNull.Value),
            new SqlParameter("@Gender", model.Gender ?? (object)DBNull.Value),
            new SqlParameter("@AadharNo", model.AadharNo ?? (object)DBNull.Value),
            new SqlParameter("@PancardNo", model.PancardNo ?? (object)DBNull.Value),
            new SqlParameter("@DrivingLicence", model.DrivingLicence ?? (object)DBNull.Value),
            new SqlParameter("@PassportNo", model.PassportNo ?? (object)DBNull.Value),
            new SqlParameter("@RationCard", model.RationCard ?? (object)DBNull.Value),
            new SqlParameter("@VoterId", model.VoterId),
            new SqlParameter("@Others", model.Others ?? (object)DBNull.Value),
              new SqlParameter("@NRI", model.Nri ?? (object)DBNull.Value),
            new SqlParameter("@BloodGroup", model.BloodGroup ?? (object)DBNull.Value),
            new SqlParameter("@PrimaryUser", model.PrimaryUser),
            new SqlParameter("@DOB", model.Dob ?? (object)DBNull.Value),
            new SqlParameter("@FoodPref", model.FoodPref ?? (object)DBNull.Value),
            new SqlParameter("@CreatedBy", model.UserId)
        };

                await _context.Database.ExecuteSqlRawAsync(sql, parameters);

                result.Message = "Saved successfully";
                result.Status = "Success";
            }
            catch (Exception ex)
            {
                result.Message = "Error saving data: " + ex.Message;
                result.Status = "Error";
            }

            return result;
        }

    }
}
