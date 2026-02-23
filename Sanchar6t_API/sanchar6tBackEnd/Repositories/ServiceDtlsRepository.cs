using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Repositories
{
    public class ServiceDtlsRepository : IServiceDtls
    {
        private readonly Sanchar6tDbContext _context;

        public ServiceDtlsRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }

        public async Task<CommonRsult> GetServiceDtls()
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwServiceDtls.ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
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

        public async Task<CommonRsult> GetServiceDtlsByID(int ServiceDtlsID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwServiceDtls.Where(m => m.ServiceId == ServiceDtlsID).ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
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

        public async Task<CommonRsult> GetServiceDtlsByPackageID(int PackageID)
        {
            CommonRsult result = new CommonRsult();
            try
            {
                var data = await _context.VwServiceDtls.Where(m => m.PackageId == PackageID).ToListAsync();
                result.Type = "S";
                result.Message = "Successfully";
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
        

        public async Task<CommonRsult> SaveServiceDtls(EServiceDtls serviceDtls)
        {
            CommonRsult result = new CommonRsult();
            try
            {                          //exception handling
                DataTable dt = new DataTable();
                var con = (SqlConnection)_context.Database.GetDbConnection();
                using (var cmd = new SqlCommand("dbo.sp_ServiceDtls", con))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@Flag", serviceDtls.Flag);
                    cmd.Parameters.AddWithValue("@ServiceID", serviceDtls.ServiceID);
                    cmd.Parameters.AddWithValue("@PackageID", serviceDtls.PackageID);
                    cmd.Parameters.AddWithValue("@Servicename", serviceDtls.Servicename);
                    cmd.Parameters.AddWithValue("@BusType", serviceDtls.BusType);
                    cmd.Parameters.AddWithValue("@Departure", serviceDtls.Departure);
                    cmd.Parameters.AddWithValue("@Duration", serviceDtls.Duration);
                    cmd.Parameters.AddWithValue("@Arrival", serviceDtls.Arrival);
                    cmd.Parameters.AddWithValue("@Fare", serviceDtls.Fare);
                    cmd.Parameters.AddWithValue("@WdFrom", serviceDtls.WdFrom);
                    cmd.Parameters.AddWithValue("@WdTo", serviceDtls.WdTo);
                    cmd.Parameters.AddWithValue("@WdFare", serviceDtls.WdFare);
                    cmd.Parameters.AddWithValue("@WeFrom", serviceDtls.WeFrom);
                    cmd.Parameters.AddWithValue("@WeTo", serviceDtls.WeTo);
                    cmd.Parameters.AddWithValue("@WeFare", serviceDtls.WeFare);
                    cmd.Parameters.AddWithValue("@CreatedBy", serviceDtls.CreatedBy);


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
