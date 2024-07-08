using Microsoft.AspNetCore.Identity;

namespace HotelListingAPI.Model
{
    public class ApiUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        //public string Role {  get; set; }
    }
}
