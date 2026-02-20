using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class AgentDtlsRepository : IAgentDtls
    {
        private readonly Sanchar6tDbContext context;

        public AgentDtlsRepository(Sanchar6tDbContext context)
        {
            this.context = context;
        }

        public async Task<CommonRsult> AgentDtls(EAgentDtls agentDtls)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_AgentDtls", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", agentDtls.Flag);
                    cmd.Parameters.AddWithValue("@AgentDtlID", agentDtls.AgentDtlID);
                    cmd.Parameters.AddWithValue("@UserID", agentDtls.UserID);
                    cmd.Parameters.AddWithValue("@FirstName ", agentDtls.FirstName);
                    cmd.Parameters.AddWithValue("@MiddleName", agentDtls.MiddleName);
                    cmd.Parameters.AddWithValue("@LastName", agentDtls.LastName);
                    cmd.Parameters.AddWithValue("@PhoneNumber", agentDtls.PhoneNumber);
                    cmd.Parameters.AddWithValue("@CompanyName ", agentDtls.CompanyName);
                    cmd.Parameters.AddWithValue("@CompanyID", agentDtls.CompanyID);
                    cmd.Parameters.AddWithValue("@CompanyAddress", agentDtls.CompanyAddress);
                    cmd.Parameters.AddWithValue("@ShopAddress", agentDtls.ShopAddress);
                    cmd.Parameters.AddWithValue("@GST", agentDtls.GST);
                    cmd.Parameters.AddWithValue("@Email", agentDtls.Email);
                    cmd.Parameters.AddWithValue("@Organisation", agentDtls.Organisation);
                    cmd.Parameters.AddWithValue("@City", agentDtls.City);
                    cmd.Parameters.AddWithValue("@State", agentDtls.State);
                    cmd.Parameters.AddWithValue("@Comments", agentDtls.Comments);
                    cmd.Parameters.AddWithValue("@Status", agentDtls.Status);
                    cmd.Parameters.AddWithValue("@CreatedBy", agentDtls.CreatedBy);


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
