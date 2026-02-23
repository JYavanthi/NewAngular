using Microsoft.Data.SqlClient;
using System.Data;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class StateRepository : IState
    {
        private readonly Sanchar6tDbContext _context;

        public StateRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }
        public async Task<CommonRsult> GetState()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwStates.ToListAsync();

                result.Data = data.OrderByDescending(m => m.Id);
                result.Message = "Successfully";
                result.Count = data.Count();
                result.Type = "S";

            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetStatebycountryID(int countryID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwStates.Where(a => a.CountryId == countryID).ToListAsync();

                result.Data = data;
                result.Message = "Successfully";
                result.Count = data.Count();
                result.Type = "S";

            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> GetStateByID(int stateID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwStates.Where(a => a.Id == stateID).ToListAsync();

                result.Data = data;
                result.Message = "Successfully";
                result.Count = data.Count();
                result.Type = "S";

            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }
            return result;
        }

        public async Task<CommonRsult> SaveState(EState eState)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_states", con))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", eState.Flag);
                    cmd.Parameters.AddWithValue("@ID", eState.ID);
                    cmd.Parameters.AddWithValue("@name", eState.name);
                    cmd.Parameters.AddWithValue("@country_id", eState.country_id);
                    cmd.Parameters.AddWithValue("@state_code", eState.state_code);
                    cmd.Parameters.AddWithValue("@type", eState.type);
                    cmd.Parameters.AddWithValue("@latitude", eState.latitude);
                    cmd.Parameters.AddWithValue("@longitude", eState.longitude);
                    cmd.Parameters.AddWithValue("@CreatedBy", eState.CreatedBy);


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

