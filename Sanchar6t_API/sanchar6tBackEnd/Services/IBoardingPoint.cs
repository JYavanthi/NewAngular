using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBoardingPoint
    {
        Task<CommonRsult> SaveBoardingPoint(EBoardingPoint eBoardingPoint);
        Task<CommonRsult> GetBoardingPoint();
    }
}
