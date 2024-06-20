using HotelListingAPI.Model.DTOs;

namespace HotelListingAPI.Services
{
    public interface IAuthManager
    {
        Task<bool> ValidateUser(LoginUserDto loginUser);
        Task<string> CreateToken();
    }
}
