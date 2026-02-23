using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IPkgHighlight
    {
        Task<CommonRsult> SavePkgHighlight(EPkgHighlight pkghighlight);
        Task<CommonRsult> GetPkgHighlight();
        Task<CommonRsult> GetPkgHighlightByID(int PackageID);
    }
}
 