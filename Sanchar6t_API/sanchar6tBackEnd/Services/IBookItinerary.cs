using sanchar6tBackEnd.Data.Entities;

namespace sanchar6tBackEnd.Services
{
    public interface IBookItinerary
    {
        Task<CommonRsult> BookItinerarydetails(EBookItinerary bookItinerary);


    }
}

 