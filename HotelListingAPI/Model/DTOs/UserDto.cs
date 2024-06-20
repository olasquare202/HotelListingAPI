using System.ComponentModel.DataAnnotations;

namespace HotelListingAPI.Model.DTOs
{
    public class UserDto : LoginUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; }
        public ICollection<string> Roles { get; set;}
        
    }
}
