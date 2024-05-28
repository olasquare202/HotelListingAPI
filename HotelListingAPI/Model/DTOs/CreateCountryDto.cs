using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Model.DTOs
{
    public class CreateCountryDto
    {
        [Required]
        [StringLength(maximumLength: 20, ErrorMessage = "Country name must not be more than 20 characters")]
        public string Name { get; set; }
        [Required]
        [StringLength(maximumLength: 3, ErrorMessage = "Short name must not be more than 3 characters")]
        public string ShortName { get; set; }
    }
}
