using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IReviews
    {
        Task<CommonRsult> SaveReviews(EReviews reviews);
        Task<CommonRsult> GetReviews();
    }
}
