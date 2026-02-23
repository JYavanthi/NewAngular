using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Data.Entities;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReviewsController : ControllerBase
    {
        private readonly IReviews _reviews;
        private readonly Sanchar6tDbContext _context;

        public ReviewsController(IReviews reviews, Sanchar6tDbContext context)
        {
            this._reviews = reviews;
            this._context = context;
        }
        [HttpGet("GetReviews")]
        public async Task<CommonRsult> GetReviews()
        {
            return await _reviews.GetReviews();
        }
        [HttpPost("SaveReviews")]
        public async Task<CommonRsult> Reviews(EReviews reviews)
        {
            
            var data1 = await _reviews.SaveReviews(reviews);
            return data1;
        }
    }
}
