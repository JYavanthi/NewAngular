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
                //var data = await _context.VwSavedPassengerDtls.FirstOrDefaultAsync(x => x.UserId == UserID);
                //result.Data = data;
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
                // SQL command to execute the stored procedure.
                var sql = "EXEC sp_SavedPassengerDtls @Flag = {0}, @PassengerDtlID = {1}, @UserID = {2}, @FirstName = {3}, @MiddleName = {4}, " +
                          "@LastName = {5}, @Email = {6}, @ContactNo = {7}, @Gender = {8}, @AadharNo = {9}, @PancardNo = {10}, " +
                          "@BloodGroup = {11}, @PrimaryUser = {12}, @DOB = {13}, @FoodPref = {14}, @CreatedBy = {15}";

                // Executing the SQL command with the provided parameters.
                await _context.Database.ExecuteSqlRawAsync(sql,
     "U", model.PassengerDtlId, model.UserId, model.FirstName, model.MiddleName,
     model.LastName, model.Email, model.ContactNo, model.Gender, model.AadharNo,
     //model.PancardNo, model.BloodGroup, model.PrimaryUser, model.Dob,
     model.FoodPref ?? model.FirstName, model.UserId);


                //result.Message = "Saved successfully";
                //result.Status = "Success"; // Ensure that 'Status' is a valid property in 'CommonRsult'
            }
            catch (Exception ex)
            {
                //result.Message = "Error saving data: " + ex.Message;
                //result.Status = "Error"; // Ensure that 'Status' is a valid property in 'CommonRsult'
            }

            return result;
        }
    }
}
