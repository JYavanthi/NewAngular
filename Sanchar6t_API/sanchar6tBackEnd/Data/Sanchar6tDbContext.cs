using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using sanchar6tBackEnd.Models;

namespace sanchar6tBackEnd.Data;

public partial class Sanchar6tDbContext : DbContext
{
    public Sanchar6tDbContext()
    {
    }

    public Sanchar6tDbContext(DbContextOptions<Sanchar6tDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AgentDtl> AgentDtls { get; set; }

    public virtual DbSet<Amenity> Amenities { get; set; }

    public virtual DbSet<Area> Areas { get; set; }

    public virtual DbSet<Attachment> Attachments { get; set; }

    public virtual DbSet<BoardingPoint> BoardingPoints { get; set; }

    public virtual DbSet<BusAmenity> BusAmenities { get; set; }

    public virtual DbSet<BusBookingDetail> BusBookingDetails { get; set; }

    public virtual DbSet<BusBookingSeat> BusBookingSeats { get; set; }

    public virtual DbSet<BusMasterCode> BusMasterCodes { get; set; }

    public virtual DbSet<BusOperator> BusOperators { get; set; }

    public virtual DbSet<City> Cities { get; set; }

    public virtual DbSet<Country> Countries { get; set; }

    public virtual DbSet<DoctorLog> DoctorLogs { get; set; }

    public virtual DbSet<Package> Packages { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Phonepe> Phonepes { get; set; }

    public virtual DbSet<PkgHighlight> PkgHighlights { get; set; }

    public virtual DbSet<PkgImageDtl> PkgImageDtls { get; set; }

    public virtual DbSet<PkgImpNote> PkgImpNotes { get; set; }

    public virtual DbSet<PkgInclude> PkgIncludes { get; set; }

    public virtual DbSet<PkgItinerary> PkgItineraries { get; set; }

    public virtual DbSet<PkgOffer> PkgOffers { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<SavedPassengerDtl> SavedPassengerDtls { get; set; }

    public virtual DbSet<ServiceDtl> ServiceDtls { get; set; }

    public virtual DbSet<State> States { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLog> UserLogs { get; set; }

    public virtual DbSet<UserSearch> UserSearches { get; set; }

    public virtual DbSet<UserSecurity> UserSecurities { get; set; }

    public virtual DbSet<UserVisitedPg> UserVisitedPgs { get; set; }

    public virtual DbSet<Usertype> Usertypes { get; set; }

    public virtual DbSet<VwAgentDtl> VwAgentDtls { get; set; }

    public virtual DbSet<VwAmenity> VwAmenities { get; set; }

    public virtual DbSet<VwArea> VwAreas { get; set; }

    public virtual DbSet<VwBoardingPoint> VwBoardingPoints { get; set; }

    public virtual DbSet<VwBusAmenity> VwBusAmenities { get; set; }

    public virtual DbSet<VwBusBookingDetail> VwBusBookingDetails { get; set; }

    public virtual DbSet<VwBusBookingSeat> VwBusBookingSeats { get; set; }

    public virtual DbSet<VwBusOperator> VwBusOperators { get; set; }

    public virtual DbSet<VwCountry> VwCountries { get; set; }

    public virtual DbSet<VwPackage> VwPackages { get; set; }

    public virtual DbSet<VwPayment> VwPayments { get; set; }

    public virtual DbSet<VwPkgHighlight> VwPkgHighlights { get; set; }

    public virtual DbSet<VwPkgImageDtl> VwPkgImageDtls { get; set; }

    public virtual DbSet<VwPkgImpNote> VwPkgImpNotes { get; set; }

    public virtual DbSet<VwPkgInclude> VwPkgIncludes { get; set; }

    public virtual DbSet<VwPkgItinerary> VwPkgItineraries { get; set; }

    public virtual DbSet<VwPkgOffer> VwPkgOffers { get; set; }

    public virtual DbSet<VwReview> VwReviews { get; set; }

    public virtual DbSet<VwServiceDtl> VwServiceDtls { get; set; }

    public virtual DbSet<VwState> VwStates { get; set; }

    public virtual DbSet<VwUser> VwUsers { get; set; }

    public virtual DbSet<VwUserDetail> VwUserDetails { get; set; }

    public virtual DbSet<VwUserLog> VwUserLogs { get; set; }

    public virtual DbSet<VwUserSearch> VwUserSearches { get; set; }

    public virtual DbSet<VwUsertype> VwUsertypes { get; set; }

    public virtual DbSet<VwWallet> VwWallets { get; set; }

    public virtual DbSet<VwWalletTransaction> VwWalletTransactions { get; set; }

    public virtual DbSet<Wallet> Wallets { get; set; }

    public virtual DbSet<WalletHistory> WalletHistories { get; set; }

    public virtual DbSet<WalletTransaction> WalletTransactions { get; set; }

    public virtual DbSet<WalletTransactionHistory> WalletTransactionHistories { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=DESKTOP-64SG9RE\\SQLEXPRESS;Database=Sanchar6t_Dev;Trusted_Connection=True;TrustServerCertificate=True");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AgentDtl>(entity =>
        {
            entity.Property(e => e.AgentDtlId).HasColumnName("AgentDtlID");
            entity.Property(e => e.City)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CompanyAddress).IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Gst)
                .HasMaxLength(1500)
                .IsUnicode(false)
                .HasColumnName("GST");
            entity.Property(e => e.LastName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Organisation).IsUnicode(false);
            entity.Property(e => e.ShopAddress).IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Amenity>(entity =>
        {
            entity.HasKey(e => e.AmenityId).HasName("PK__Amenity__842AF52B819D30CC");

            entity.ToTable("Amenity");

            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.Amicon)
                .HasMaxLength(250)
                .HasColumnName("AMIcon");
            entity.Property(e => e.Amname)
                .HasMaxLength(250)
                .HasColumnName("AMName");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
        });

        modelBuilder.Entity<Area>(entity =>
        {
            entity.ToTable("Area");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.AreaName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.Property(e => e.AttachmentId).HasColumnName("AttachmentID");
            entity.Property(e => e.AttachmentFile)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.AttachmentName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Section)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<BoardingPoint>(entity =>
        {
            entity.HasKey(e => e.BoardingPointId).HasName("PK__Boarding__978F314F62E026E5");

            entity.ToTable("BoardingPoint");

            entity.Property(e => e.BoardingPointId).HasColumnName("BoardingPointID");
            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Landmark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Town)
                .HasMaxLength(100)
                .IsUnicode(false);

            entity.HasOne(d => d.Area).WithMany(p => p.BoardingPoints)
                .HasForeignKey(d => d.AreaId)
                .HasConstraintName("FK_BoardingPoint_Area");
        });

        modelBuilder.Entity<BusAmenity>(entity =>
        {
            entity.HasKey(e => e.BusAmenitiesId).HasName("PK__BusAmeni__9DBC8411BC3C8D9D");

            entity.Property(e => e.BusAmenitiesId).HasColumnName("BusAmenitiesID");
            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.BusOperatorId).HasColumnName("BusOperatorID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");

            entity.HasOne(d => d.Amenity).WithMany(p => p.BusAmenities)
                .HasForeignKey(d => d.AmenityId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BusAmenities_Amenity");

            entity.HasOne(d => d.BusOperator).WithMany(p => p.BusAmenities)
                .HasForeignKey(d => d.BusOperatorId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("FK_BusAmenities_BusOperator");
        });

        modelBuilder.Entity<BusBookingDetail>(entity =>
        {
            entity.HasKey(e => e.BusBooKingDetailId).HasName("PK__BusBooki__EC4212D09E554E6C");

            entity.Property(e => e.BusBooKingDetailId).HasColumnName("BusBooKingDetailID");
            entity.Property(e => e.AgentId).HasColumnName("AgentID");
            entity.Property(e => e.Arrivaltime).HasColumnType("datetime");
            entity.Property(e => e.BusNum).HasMaxLength(50);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.OperatorId).HasColumnName("OperatorID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.SeatPrice).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<BusBookingSeat>(entity =>
        {
            entity.HasKey(e => e.BusBookingSeatId).HasName("PK__BusBooki__6B0FC86FE51C5D40");

            entity.ToTable("BusBookingSeat");

            entity.Property(e => e.BusBookingSeatId).HasColumnName("BusBookingSeatID");
            entity.Property(e => e.AadharNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FoodPref)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PancardNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.SeatNo).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<BusMasterCode>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("BusMasterCode");

            entity.Property(e => e.Code)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MasterCodeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("MasterCodeID");
        });

        modelBuilder.Entity<BusOperator>(entity =>
        {
            entity.HasKey(e => e.BusOperatorId).HasName("PK__BusOpera__1D5D1039A3ED093E");

            entity.ToTable("BusOperator");

            entity.Property(e => e.BusOperatorId).HasColumnName("BusOperatorID");
            entity.Property(e => e.BusNo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusSeats)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BusType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.FemaleSeatNo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MaleSeatNo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.ToTable("cities");

            entity.Property(e => e.Id)
                .ValueGeneratedNever()
                .HasColumnName("id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .HasColumnName("state_code");
            entity.Property(e => e.StateId).HasColumnName("state_id");
            entity.Property(e => e.StateName)
                .HasMaxLength(50)
                .HasColumnName("state_name");
            entity.Property(e => e.Town)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.WikiDataId)
                .HasColumnType("money")
                .HasColumnName("wikiDataId");
        });

        modelBuilder.Entity<Country>(entity =>
        {
            entity.ToTable("countries");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Capital)
                .HasMaxLength(50)
                .HasColumnName("capital");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("currency");
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .HasColumnName("currency_name");
            entity.Property(e => e.CurrencySymbol)
                .HasMaxLength(50)
                .HasColumnName("currency_symbol");
            entity.Property(e => e.Emoji)
                .HasMaxLength(50)
                .HasColumnName("emoji");
            entity.Property(e => e.EmojiU)
                .HasMaxLength(50)
                .HasColumnName("emojiU");
            entity.Property(e => e.Iso2)
                .HasMaxLength(50)
                .HasColumnName("iso2");
            entity.Property(e => e.Iso3)
                .HasMaxLength(50)
                .HasColumnName("iso3");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.Native)
                .HasMaxLength(100)
                .HasColumnName("native");
            entity.Property(e => e.NumericCode).HasColumnName("numeric_code");
            entity.Property(e => e.Phonecode).HasColumnName("phonecode");
            entity.Property(e => e.Region)
                .HasMaxLength(50)
                .HasColumnName("region");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Subregion)
                .HasMaxLength(50)
                .HasColumnName("subregion");
            entity.Property(e => e.SubregionId).HasColumnName("subregion_id");
            entity.Property(e => e.Timezones).HasColumnName("timezones");
            entity.Property(e => e.Tld)
                .HasMaxLength(50)
                .HasColumnName("tld");
        });

        modelBuilder.Entity<DoctorLog>(entity =>
        {
            entity.HasKey(e => e.DoctorlogId).HasName("PK__DoctorLo__2E8573BADE447813");

            entity.Property(e => e.DoctorlogId).HasColumnName("DoctorlogID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.DoctorId).HasColumnName("DoctorID");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(2000);
        });

        modelBuilder.Entity<Package>(entity =>
        {
            entity.HasKey(e => e.PackageId).HasName("PK__Package__322035EC8F7B475D");

            entity.ToTable("Package");

            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.AdditionalNotes).IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PackagePrice).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Shortdescription).IsUnicode(false);
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.PaymentId).HasName("PK__Payment__9B556A58976BE707");

            entity.ToTable("Payment");

            entity.Property(e => e.PaymentId).HasColumnName("PaymentID");
            entity.Property(e => e.BookingdtlsId).HasColumnName("BookingdtlsID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId)
                .IsUnicode(false)
                .HasColumnName("TransactionID");
            entity.Property(e => e.TransactionResponse).IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<Phonepe>(entity =>
        {
            entity.HasKey(e => e.PhonepeId).HasName("PK__Phonepe__CE06DC6F6D5882CD");

            entity.ToTable("Phonepe");

            entity.Property(e => e.PhonepeId).HasColumnName("PhonepeID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ExpireAt)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.MerchantOrderId)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.OrderId)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.RedirectUrl).IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<PkgHighlight>(entity =>
        {
            entity.HasKey(e => e.PkgHighlightId).HasName("PK__PkgHighl__648E61F7D41D3525");

            entity.ToTable("PkgHighlight");

            entity.Property(e => e.PkgHighlightId).HasColumnName("PkgHighlightID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Title).HasMaxLength(450);
        });

        modelBuilder.Entity<PkgImageDtl>(entity =>
        {
            entity.HasKey(e => e.PkgImageId).HasName("PK__PkgImage__30DDE01FD31A1518");

            entity.Property(e => e.PkgImageId).HasColumnName("PkgImageID");
            entity.Property(e => e.BtnName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BtnUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Heading)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PkgSection)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SubHeading)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<PkgImpNote>(entity =>
        {
            entity.HasKey(e => e.PkgImpNoteId).HasName("PK__PkgImpNo__6847FABC5582758D");

            entity.Property(e => e.PkgImpNoteId).HasColumnName("PkgImpNoteID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
        });

        modelBuilder.Entity<PkgInclude>(entity =>
        {
            entity.HasKey(e => e.PkgIncludeId).HasName("PK__PkgInclu__FE3C65F1985CA0C7");

            entity.ToTable("PkgInclude");

            entity.Property(e => e.PkgIncludeId).HasColumnName("PkgIncludeID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
        });

        modelBuilder.Entity<PkgItinerary>(entity =>
        {
            entity.HasKey(e => e.PkgItineraryId).HasName("PK__PkgItine__A91BB618EBF7EA6E");

            entity.ToTable("PkgItinerary");

            entity.Property(e => e.PkgItineraryId).HasColumnName("PkgItineraryID");
            entity.Property(e => e.CreateDt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<PkgOffer>(entity =>
        {
            entity.HasKey(e => e.PkgOfferId).HasName("PK__PkgOffer__0006C2E2A629323C");

            entity.ToTable("PkgOffer");

            entity.Property(e => e.PkgOfferId).HasColumnName("PkgOfferID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PK__Reviews__74BC79AEA6E46594");

            entity.Property(e => e.ReviewId).HasColumnName("ReviewID");
            entity.Property(e => e.BusBooKingDetailId).HasColumnName("BusBooKingDetailID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<SavedPassengerDtl>(entity =>
        {
            entity.HasKey(e => e.PassengerDtlId).HasName("PK__SavedPas__5C61E2E2070E9F13");

            entity.Property(e => e.PassengerDtlId).HasColumnName("PassengerDtlID");
            entity.Property(e => e.AadharNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Address).IsUnicode(false);
            entity.Property(e => e.Age)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.AlternativeNumber)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FoodPref)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PancardNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Remarks)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<ServiceDtl>(entity =>
        {
            entity.HasKey(e => e.ServiceId).HasName("PK__ServiceD__C51BB0EA2FB3BE31");

            entity.Property(e => e.ServiceId).HasColumnName("ServiceID");
            entity.Property(e => e.BusType).HasMaxLength(300);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasMaxLength(300);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.Servicename).HasMaxLength(350);
            entity.Property(e => e.WdFrom).HasMaxLength(250);
            entity.Property(e => e.WdTo).HasMaxLength(250);
            entity.Property(e => e.WeFrom).HasMaxLength(1);
            entity.Property(e => e.WeTo).HasMaxLength(250);
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.ToTable("states");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .HasColumnName("state_code");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PK__User__1788CCACF457C90C");

            entity.ToTable("User");

            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserLog>(entity =>
        {
            entity.HasKey(e => e.UserlogId).HasName("PK__UserLogs__4055E6C1258FA3DA");

            entity.Property(e => e.UserlogId).HasColumnName("UserlogID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(2000);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UserSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("UserSearch");

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModeOfTransport)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Operator)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SearchedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<UserSecurity>(entity =>
        {
            entity.HasKey(e => e.UserSecurityId).HasName("PK__UserSecu__3029CED1BBA64DD0");

            entity.ToTable("UserSecurity");

            entity.Property(e => e.UserSecurityId).HasColumnName("UserSecurityID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<UserVisitedPg>(entity =>
        {
            entity.Property(e => e.UserVisitedPgId).HasColumnName("UserVisitedPgID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.VisitedPgTimeFrom).HasColumnType("datetime");
            entity.Property(e => e.VisitedPgTimeTo).HasColumnType("datetime");
        });

        modelBuilder.Entity<Usertype>(entity =>
        {
            entity.HasKey(e => e.UsertypeId).HasName("PK__Usertype__F6AC949176A8213B");

            entity.ToTable("Usertype");

            entity.Property(e => e.UsertypeId).HasColumnName("UsertypeID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Usertype1)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Usertype");
        });

        modelBuilder.Entity<VwAgentDtl>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_AgentDtls");

            entity.Property(e => e.AgentDtlId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AgentDtlID");
            entity.Property(e => e.City)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.Comments).IsUnicode(false);
            entity.Property(e => e.CompanyAddress).IsUnicode(false);
            entity.Property(e => e.CompanyId).HasColumnName("CompanyID");
            entity.Property(e => e.CompanyName)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.ContactNo).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Gst)
                .HasMaxLength(1500)
                .IsUnicode(false)
                .HasColumnName("GST");
            entity.Property(e => e.LastName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Organisation).IsUnicode(false);
            entity.Property(e => e.ShopAddress).IsUnicode(false);
            entity.Property(e => e.State)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwAmenity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Amenity");

            entity.Property(e => e.AmenityId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AmenityID");
            entity.Property(e => e.Amicon)
                .HasMaxLength(250)
                .HasColumnName("AMIcon");
            entity.Property(e => e.Amname)
                .HasMaxLength(250)
                .HasColumnName("AMName");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwArea>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Area");

            entity.Property(e => e.AreaId)
                .ValueGeneratedOnAdd()
                .HasColumnName("AreaID");
            entity.Property(e => e.AreaName)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.CityId).HasColumnName("CityID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Pincode)
                .HasMaxLength(10)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwBoardingPoint>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BoardingPoint");

            entity.Property(e => e.AreaId).HasColumnName("AreaID");
            entity.Property(e => e.BoardingPointId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BoardingPointID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Landmark)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Town)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwBusAmenity>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BusAmenities");

            entity.Property(e => e.AmenityId).HasColumnName("AmenityID");
            entity.Property(e => e.BusAmenitiesId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BusAmenitiesID");
            entity.Property(e => e.BusOperatorId).HasColumnName("BusOperatorID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwBusBookingDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BusBookingDetails");

            entity.Property(e => e.AgentId).HasColumnName("AgentID");
            entity.Property(e => e.ArrivalTime).HasColumnType("datetime");
            entity.Property(e => e.BusBooKingDetailId).HasColumnName("BusBooKingDetailID");
            entity.Property(e => e.BusNum).HasMaxLength(50);
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.DepartureTime).HasColumnType("datetime");
            entity.Property(e => e.FromDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.OperatorId).HasColumnName("OperatorID");
            entity.Property(e => e.ScheduleId).HasColumnName("ScheduleID");
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ToDate).HasColumnType("datetime");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwBusBookingSeat>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BusBookingSeat");

            entity.Property(e => e.AadharNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.BloodGroup)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusBookingSeatId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BusBookingSeatID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Dob)
                .HasColumnType("datetime")
                .HasColumnName("DOB");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.FoodPref)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PancardNo)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.SeatNo).HasMaxLength(50);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwBusOperator>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_BusOperator");

            entity.Property(e => e.BusNo)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.BusOperatorId)
                .ValueGeneratedOnAdd()
                .HasColumnName("BusOperatorID");
            entity.Property(e => e.BusSeats)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.BusType)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.FemaleSeatNo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.MaleSeatNo)
                .HasMaxLength(150)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwCountry>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_countries");

            entity.Property(e => e.Capital)
                .HasMaxLength(50)
                .HasColumnName("capital");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Currency)
                .HasMaxLength(50)
                .HasColumnName("currency");
            entity.Property(e => e.CurrencyName)
                .HasMaxLength(50)
                .HasColumnName("currency_name");
            entity.Property(e => e.CurrencySymbol)
                .HasMaxLength(50)
                .HasColumnName("currency_symbol");
            entity.Property(e => e.Emoji)
                .HasMaxLength(50)
                .HasColumnName("emoji");
            entity.Property(e => e.EmojiU)
                .HasMaxLength(50)
                .HasColumnName("emojiU");
            entity.Property(e => e.Iso2)
                .HasMaxLength(50)
                .HasColumnName("iso2");
            entity.Property(e => e.Iso3)
                .HasMaxLength(50)
                .HasColumnName("iso3");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.Nationality)
                .HasMaxLength(50)
                .HasColumnName("nationality");
            entity.Property(e => e.Native)
                .HasMaxLength(100)
                .HasColumnName("native");
            entity.Property(e => e.NumericCode).HasColumnName("numeric_code");
            entity.Property(e => e.Phonecode).HasColumnName("phonecode");
            entity.Property(e => e.Region)
                .HasMaxLength(50)
                .HasColumnName("region");
            entity.Property(e => e.RegionId).HasColumnName("region_id");
            entity.Property(e => e.Subregion)
                .HasMaxLength(50)
                .HasColumnName("subregion");
            entity.Property(e => e.SubregionId).HasColumnName("subregion_id");
            entity.Property(e => e.Timezones).HasColumnName("timezones");
            entity.Property(e => e.Tld)
                .HasMaxLength(50)
                .HasColumnName("tld");
        });

        modelBuilder.Entity<VwPackage>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Package");

            entity.Property(e => e.AdditionalNotes).IsUnicode(false);
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModifiedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PackageName)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.PackagePrice).HasColumnType("numeric(18, 0)");
            entity.Property(e => e.Shortdescription).IsUnicode(false);
        });

        modelBuilder.Entity<VwPayment>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Payment");

            entity.Property(e => e.BookingdtlsId).HasColumnName("BookingdtlsID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PaymentId)
                .ValueGeneratedOnAdd()
                .HasColumnName("PaymentID");
            entity.Property(e => e.PaymentMode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.PaymentStatus)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.TransactionId).HasColumnName("TransactionID");
            entity.Property(e => e.TransactionResponse).IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwPkgHighlight>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgHighlight");

            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1500);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedbyByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgHighlightId).HasColumnName("PkgHighlightID");
            entity.Property(e => e.Title).HasMaxLength(450);
        });

        modelBuilder.Entity<VwPkgImageDtl>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgImageDtls");

            entity.Property(e => e.BtnName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.BtnUrl)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Heading)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedbyByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgImage)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.PkgImageId).HasColumnName("PkgImageID");
            entity.Property(e => e.PkgSection)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.SubHeading)
                .HasMaxLength(1000)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwPkgImpNote>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgImpNotes");

            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).HasMaxLength(1000);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedbyByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgImpNoteId).HasColumnName("PkgImpNoteID");
        });

        modelBuilder.Entity<VwPkgInclude>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgInclude");

            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description)
                .HasMaxLength(1000)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedbyByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgIncludeId).HasColumnName("PkgIncludeID");
        });

        modelBuilder.Entity<VwPkgItinerary>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgItinerary");

            entity.Property(e => e.CreateDt).HasColumnType("datetime");
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.Description)
                .HasMaxLength(1500)
                .IsUnicode(false);
            entity.Property(e => e.FromTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgItineraryId).HasColumnName("PkgItineraryID");
            entity.Property(e => e.Title)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ToTime).HasColumnType("datetime");
        });

        modelBuilder.Entity<VwPkgOffer>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_PkgOffer");

            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.EffectiveDate).HasColumnType("datetime");
            entity.Property(e => e.ModifiedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.PkgOfferId).HasColumnName("PkgOfferID");
            entity.Property(e => e.Price).HasColumnType("numeric(18, 0)");
        });

        modelBuilder.Entity<VwReview>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Reviews");

            entity.Property(e => e.BusBooKingDetailId).HasColumnName("BusBooKingDetailID");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Description).IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.ReviewId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ReviewID");
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwServiceDtl>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_ServiceDtls");

            entity.Property(e => e.BusType).HasMaxLength(300);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Duration).HasMaxLength(300);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.PackageId).HasColumnName("PackageID");
            entity.Property(e => e.ServiceId)
                .ValueGeneratedOnAdd()
                .HasColumnName("ServiceID");
            entity.Property(e => e.Servicename).HasMaxLength(350);
            entity.Property(e => e.WdFrom).HasMaxLength(250);
            entity.Property(e => e.WdTo).HasMaxLength(250);
            entity.Property(e => e.WeFrom).HasMaxLength(1);
            entity.Property(e => e.WeTo).HasMaxLength(250);
        });

        modelBuilder.Entity<VwState>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_State");

            entity.Property(e => e.CountryCode)
                .HasMaxLength(50)
                .HasColumnName("country_code");
            entity.Property(e => e.CountryId).HasColumnName("country_id");
            entity.Property(e => e.CountryName)
                .HasMaxLength(50)
                .HasColumnName("country_name");
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnName("id");
            entity.Property(e => e.Latitude).HasColumnName("latitude");
            entity.Property(e => e.Longitude).HasColumnName("longitude");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
            entity.Property(e => e.StateCode)
                .HasMaxLength(50)
                .HasColumnName("state_code");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("type");
        });

        modelBuilder.Entity<VwUser>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_User");

            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserTypeId).HasColumnName("UserTypeID");
            entity.Property(e => e.Usertype)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<VwUserDetail>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserDetails");

            entity.Property(e => e.ContactNo)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.CreatedByName)
                .HasMaxLength(752)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Email)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.FirstName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Gender)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.LastName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.MiddleName)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Password)
                .HasMaxLength(2000)
                .IsUnicode(false);
            entity.Property(e => e.Status)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<VwUserLog>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserLogs");

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.LoginTime).HasColumnType("datetime");
            entity.Property(e => e.LogoutTime).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Token).HasMaxLength(2000);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.UserlogId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserlogID");
        });

        modelBuilder.Entity<VwUserSearch>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_UserSearch");

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModeOfTransport)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Operator)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.SearchedDate).HasColumnType("datetime");
            entity.Property(e => e.UserId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UserID");
        });

        modelBuilder.Entity<VwUsertype>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Usertype");

            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.Usertype)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.UsertypeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("UsertypeID");
        });

        modelBuilder.Entity<VwWallet>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_Wallet");

            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionLimit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WalletId)
                .ValueGeneratedOnAdd()
                .HasColumnName("WalletID");
        });

        modelBuilder.Entity<VwWalletTransaction>(entity =>
        {
            entity
                .HasNoKey()
                .ToView("vw_WalletTransaction");

            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Mode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WalletTrnsnId)
                .ValueGeneratedOnAdd()
                .HasColumnName("WalletTrnsnID");
        });

        modelBuilder.Entity<Wallet>(entity =>
        {
            entity.HasKey(e => e.WalletId).HasName("PK__Wallet__84D4F92EA3EF4ED7");

            entity.ToTable("Wallet");

            entity.Property(e => e.WalletId).HasColumnName("WalletID");
            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionLimit)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<WalletHistory>(entity =>
        {
            entity.HasKey(e => e.WalletHistoryId).HasName("PK__WalletHi__161CF9651583573F");

            entity.ToTable("WalletHistory");

            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionLimit)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Type)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WalletId).HasColumnName("WalletID");
        });

        modelBuilder.Entity<WalletTransaction>(entity =>
        {
            entity.HasKey(e => e.WalletTrnsnId).HasName("PK__WalletTr__9DE8BC48CA5567F1");

            entity.ToTable("WalletTransaction");

            entity.Property(e => e.WalletTrnsnId).HasColumnName("WalletTrnsnID");
            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Mode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
        });

        modelBuilder.Entity<WalletTransactionHistory>(entity =>
        {
            entity
                .HasNoKey()
                .ToTable("WalletTransactionHistory");

            entity.Property(e => e.Amount)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.CreatedDt).HasColumnType("datetime");
            entity.Property(e => e.Date).HasColumnType("datetime");
            entity.Property(e => e.ErrorCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Message)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Mode)
                .HasMaxLength(250)
                .IsUnicode(false);
            entity.Property(e => e.ModifiedDt).HasColumnType("datetime");
            entity.Property(e => e.TransactionCode)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.TransactionNumber)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.UserId).HasColumnName("UserID");
            entity.Property(e => e.WalletTrnsnHistoryId)
                .ValueGeneratedOnAdd()
                .HasColumnName("WalletTrnsnHistoryID");
            entity.Property(e => e.WalletTrnsnId).HasColumnName("WalletTrnsnID");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
