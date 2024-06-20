using HotelListingAPI.Model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace HotelListingAPI.Configurations
{
    public class CountryConfiguration : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> builder)
        {
            builder.HasData(
                new Country
                {
                    Id = 1,
                    Name = "Jamaica",
                    ShortName = "JM"
                },
                new Country
                {
                    Id = 2,
                    Name = "Bahamas",
                    ShortName = "BS"
                },
                 new Country
                 {
                     Id = 3,
                     Name = "Cayman Island",
                     ShortName = "CI"
                 },
                 new Country
                 {
                     Id = 4,
                     Name = "Nigeria",
                     ShortName = "NG"
                 },
                 new Country
                 {
                     Id = 5,
                     Name = "South Africa",
                     ShortName = "SA"
                 });
        }//N.B: u must apply this in the DbContext class
    }
}
