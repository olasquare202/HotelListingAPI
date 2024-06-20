using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Model.DTOs
{
    public class LoginUserDto
    {
        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }
        [Required]
        [StringLength(15, ErrorMessage = "Your Password is Limited to {2} to {1} Characters", MinimumLength = 2)]
        public string Password { get; set; }
    }
}
