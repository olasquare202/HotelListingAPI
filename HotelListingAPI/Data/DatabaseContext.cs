using HotelListingAPI.Configurations;
using HotelListingAPI.Model;
using HotelListingAPI.RoleConfiguration;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace HotelListingAPI.Data
{
    public class DatabaseContext : IdentityDbContext<ApiUser>
    {
        public DatabaseContext(DbContextOptions options) : base(options) 
        {
                
        }
       
        public DbSet<Country> Countries { get; set; }
        public DbSet<Hotel> Hotels { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);//This line only applicable if we are
                                               //using Identity library(IdentityDbContext),
                                               //else we start frm d next line below(i.e //modelBuilder.Entity<Country>().HasData()
            
            modelBuilder.ApplyConfiguration(new CountryConfiguration());//Country configuration is applied here
            modelBuilder.ApplyConfiguration(new HotelConfiguration());//Hotel configuration is applied here
            modelBuilder.ApplyConfiguration(new UserRole());//D UserRole configuration is applied here
            //modelBuilder.Entity<Country>().HasData(
            //    new Country
            //    {
            //        Id = 1,
            //        Name = "Jamaica",
            //        ShortName = "JM"
            //    },
            //    new Country
            //    {
            //        Id = 2,
            //        Name = "Bahamas",
            //        ShortName = "BS"
            //    },
            //     new Country
            //     {
            //         Id = 3,
            //         Name = "Cayman Island",
            //         ShortName = "CI"
            //     }
            //    );

            //modelBuilder.Entity<Hotel>().HasData(
            //    new Hotel
            //    {
            //        Id = 1,
            //        Name = "Sandals Resort Spa",
            //        Address = "Negril",
            //        CountryId = 1,
            //        Rating = 4.5 //Although its a 5 star Hotel but I just gave it 4.5
            //    },
            //    new Hotel
            //    {
            //        Id = 2,
            //        Name = "Comfort Suites",
            //        Address = "George Town",
            //        CountryId = 3,
            //        Rating = 4.3
            //    },
            //     new Hotel
            //     {
            //         Id = 3,
            //         Name = "Grand Palldium",
            //         Address = "Nassua",
            //         CountryId = 2,
            //         Rating = 4
            //     }
            //    );
            
        }
    }
}
