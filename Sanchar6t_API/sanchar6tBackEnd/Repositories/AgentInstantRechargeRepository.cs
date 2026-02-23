using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using sanchar6tBackEnd.Helpers;


namespace sanchar6tBackEnd.Repositories
{
    public class AgentInstantRechargeRepository : IAgentInstantRechargeRepository
    {
        private readonly Sanchar6tDbContext _context;

        public AgentInstantRechargeRepository(Sanchar6tDbContext context)
        {
            _context = context;
        }

        // ================= CREATE INSTANT RECHARGE =================
        public CommonRsult Create(EAgentInstantRechargeDtl recharge)
        {
            CommonRsult result = new CommonRsult();

            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();

                using (var cmd = new SqlCommand("dbo.sp_AgentInstantRecharge_Insert", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@AgentDtlID", recharge.AgentDtlId);
                    cmd.Parameters.AddWithValue("@UserID", recharge.UserId);
                    cmd.Parameters.AddWithValue("@Amount", recharge.Amount);
                    cmd.Parameters.AddWithValue("@TransactionCharge", recharge.TransactionCharge);
                    cmd.Parameters.AddWithValue("@NetAmount", recharge.NetAmount);
                    cmd.Parameters.AddWithValue("@PaymentMode", recharge.PaymentMode ?? "");
                    cmd.Parameters.AddWithValue("@CreatedBy", recharge.CreatedBy);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                result.Type = "S";
                result.Message = "Instant recharge created successfully";
                result.Data = dt.ToList();
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }

            return result;
        }

        // ================= UPDATE STATUS =================
        public CommonRsult UpdateStatus(EAgentInstantRechargeDtl recharge)
        {
            CommonRsult result = new CommonRsult();

            try
            {
                var con = (SqlConnection)_context.Database.GetDbConnection();

                using (var cmd = new SqlCommand("dbo.sp_AgentInstantRecharge_UpdateStatus", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@InstantRechargeID", recharge.InstantRechargeId);
                    cmd.Parameters.AddWithValue("@Status", recharge.Status);
                    cmd.Parameters.AddWithValue("@ReferenceNo", recharge.ReferenceNo ?? "");
                    cmd.Parameters.AddWithValue("@Remarks", recharge.Remarks ?? "");
                    cmd.Parameters.AddWithValue("@ModifiedBy", recharge.ModifiedBy ?? recharge.CreatedBy);

                    if (con.State == ConnectionState.Closed)
                        con.Open();

                    cmd.ExecuteNonQuery();
                }

                result.Type = "S";
                result.Message = "Recharge status updated successfully";
            }
            catch (Exception ex)
            {
                result.Type = "E";
                result.Message = ex.Message;
            }

            return result;
        }

        // ================= GET BY ID =================
        public CommonRsult GetById(int instantRechargeId)
        {
            CommonRsult result = new CommonRsult();

            try
            {
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();

                using (var cmd = new SqlCommand("dbo.sp_AgentInstantRecharge_GetById", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@InstantRechargeID", instantRechargeId);

                    using (var da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(dt);
                    }
                }

                result.Type = "S";
                result.Message = "Recharge details fetched successfully";
                result.Data = dt;
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
