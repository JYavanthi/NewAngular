using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface Icountries
    {
        Task<CommonRsult> GetCountries();
        Task<CommonRsult> GetCountryByID(int countryID);
        Task<CommonRsult> Savecountries(Ecountries ecountries);


    }
}
