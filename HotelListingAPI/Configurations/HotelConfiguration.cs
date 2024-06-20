using HotelListingAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Configurations
{
    public class HotelConfiguration : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasData(
            new Hotel
            {
                Id = 1,
                Name = "Sandals Resort Spa",
                Address = "Negril",
                CountryId = 1,
                Rating = 4.5 //Although its a 5 star Hotel but I just gave it 4.5
            },
                new Hotel
                {
                    Id = 2,
                    Name = "Comfort Suites",
                    Address = "George Town",
                    CountryId = 3,
                    Rating = 4.3
                },
                 new Hotel
                 {
                     Id = 3,
                     Name = "Grand Palldium",
                     Address = "Nassua",
                     CountryId = 2,
                     Rating = 4
                 },
                 new Hotel
                 {
                     Id = 4,
                     Name = "A & T Presidential Hotel",
                     Address = "Akure, Ondo State",
                     CountryId = 4,
                     Rating = 5
                 },
                  new Hotel
                  {
                      Id = 5,
                      Name = "Tinubu Hotel",
                      Address = "Congi",
                      CountryId = 5,
                      Rating = 5
                  }
                );
        }//N.B: u must apply this in the DbContext class
    }
}
