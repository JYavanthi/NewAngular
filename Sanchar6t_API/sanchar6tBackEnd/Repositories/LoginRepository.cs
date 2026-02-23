using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Data;
using System.Text;
using System.Security.Cryptography;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using sanchar6tBackEnd.Data.Entities;
namespace sanchar6tBackEnd.Repositories
{
    public class LoginRepository : ILogin
    {
        private readonly Sanchar6tDbContext _context;

        public LoginRepository(Sanchar6tDbContext context)
        {
            this._context = context;
        }
        public async Task<CommonRsult> AuthenticateUser(string Email, string MoblieNo, string password)
        {
            if (string.IsNullOrEmpty(MoblieNo) || string.IsNullOrEmpty(password))
                return new CommonRsult { Type = "E", Message = "Username or password cannot be empty" };

            var user = await _context.VwUsers
                .Where(u => (u.ContactNo == MoblieNo || u.Email == Email) && u.Password == password)
                .ToListAsync();


            if (user == null)
            {
                return new CommonRsult { Type = "E", Message = "Invalid credentials" };
            }

            return new CommonRsult
            {
                Type = "S",
                Message = "Login successful",
                Data = user,
                Count = user.Count()
            };
        }


    }
}
