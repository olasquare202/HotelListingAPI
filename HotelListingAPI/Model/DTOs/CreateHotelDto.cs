using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HotelListingAPI.Model.DTOs
{
    public class CreateHotelDto
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Hotel name must not be more than 20 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 250, ErrorMessage = "Hotel name must not be more than 250 characters")]
        public string Address { get; set; }
        [Required]
        [Range(1, 5)]
        public double Rating { get; set; }
        //[ForeignKey(nameof(Country))]//Hotel reference d name of class 'Country'
        public int CountryId { get; set; }
       // public Country Country { get; set; }//This gives Hotel access to all d fields in class 'Country' i.e Hotel.Country.ShortName e.t.c ShortName of d Country.

    }
}
