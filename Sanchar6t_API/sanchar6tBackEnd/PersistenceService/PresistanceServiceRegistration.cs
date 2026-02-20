using sanchar6tBackEnd.Data;
using sanchar6tBackEnd.Repositories;
using sanchar6tBackEnd.Services;

namespace sanchar6tBackEnd.PersistenceService
{
    public static class PresistanceServiceRegistration
    {
        public static IServiceCollection AddPersistanceService(this IServiceCollection services
            ,IConfiguration configuration) {
            //Pre
            services.AddScoped<ILogin, LoginRepository>();
            services.AddDbContext<Sanchar6tDbContext>();
            services.AddScoped<IUser, UserRepository>();
            services.AddScoped<IPackage, PackageRepository>();//regestration of service
            //services.AddScoped<IState, StateRepository>();
            services.AddScoped<IBusBookingDetails, BusBookingDetailsRepository>();
            services.AddScoped<IBookItinerary, BookItineraryRepository>();
            services.AddScoped<IUsertype, UsertypeRepository>();
            services.AddScoped<IUserLogs, UserLogsRepository>();
            services.AddScoped<IUserVisitedPgs, UserVisitedPgsRepository>();
            services.AddScoped<IBusBookingSeat, BusBookingSeatRepository>();
            services.AddScoped<IPkgHighlight, PkgHighlightRepository>();
            services.AddScoped<IPkgImageDtls, PkgImageDtlsRepository>();
            services.AddScoped<IPkgInclude, PkgIncludeRepository>();
            services.AddScoped<IPkgItinerary, PkgItineraryRepository>();
            services.AddScoped<IPkgOffer, PkgOfferRepository>();
            services.AddScoped<IPkgImpNotes, PkgImpNotesRepository>();
            services.AddScoped<IServiceDtls, ServiceDtlsRepository>();
            services.AddScoped<IAgentDtls, AgentDtlsRepository>();
            services.AddScoped<IAmenity, AmenityRepository>();
            services.AddScoped<IBusOperator, BusOperatorRepository>();
            services.AddScoped<IBusAmenities, BusAmenitiesRepository>();
            services.AddScoped<IUserSearch, UserSearchRepository>();
            //services.AddScoped<Icities, citiesRepository>();
            services.AddScoped<Icountries, countriesRepository>();
            services.AddScoped<IArea, AreaRepository>();
            services.AddScoped<IReviews, ReviewsRepository>();
            services.AddScoped<IWallet, WalletRepository>();
            services.AddScoped<IWalletTransaction, WalletTransactionRepository>();
            services.AddScoped<IPayment, PaymentRepository>();

            return services;
        }
    }
}




