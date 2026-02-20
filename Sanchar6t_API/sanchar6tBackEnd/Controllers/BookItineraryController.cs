using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Models;
using sanchar6tBackEnd.Services;
using User = sanchar6tBackEnd.Models.VwUser;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookItineraryController : ControllerBase
    {
        private readonly IBookItinerary _bookItinerary;
        private readonly Sanchar6tDbContext _context;

        public BookItineraryController(IBookItinerary bookItinerary, Sanchar6tDbContext context)
        {
            this._bookItinerary = bookItinerary;
            this._context = context;
        }
        [HttpPost("PostBookItinerary")]
        public async Task<CommonRsult> BookItinerarydetails(EBookItinerary bookItinerary)
        {
            var userType = User.FindFirst("UserType")?.Value;
            if (userType == "User")
                return new CommonRsult { Message = "Access denied: Only admins or agent can add details", Type = "E" };
            var data1 = await _bookItinerary.BookItinerarydetails(bookItinerary);
            return data1;
        }
    }
}
  