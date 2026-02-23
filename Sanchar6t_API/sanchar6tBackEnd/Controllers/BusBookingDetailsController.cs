using Humanizer.Localisation.TimeToClockNotation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusBookingDetailsController : ControllerBase
    {
        private readonly IBusBookingDetails _bookingDetails;
        private readonly Sanchar6tDbContext _context;

        public BusBookingDetailsController(IBusBookingDetails bookingDetails, Sanchar6tDbContext context)
        {
            this._bookingDetails = bookingDetails;
            this._context = context;
        }
        [HttpGet("GetBookingDetails")]
        public async Task<CommonRsult> GetBusBookingDtls()
        {
            return await _bookingDetails.GetBusBookingDtls();
        }
        [HttpGet("GetBookingDtlByID")]
        public async Task<CommonRsult> GetBusBookingDtlByID(int BookingID)
        {
            return await _bookingDetails.GetBusBookingDtlByID(BookingID);
        }

         [HttpPost("SaveBookingDetails")]
        public async Task<CommonRsult> Bookingdetails(EBusBookingDetails bookingDetails)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data2 = await _bookingDetails.SaveBusBookingdetails(bookingDetails);
            return data2;
        }
    }
}
