using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IUserVisitedPgs
    {
        Task<CommonRsult> SaveUserVisitedPgs(EUserVisitedPgs userVisitedPgs);

    }
}
