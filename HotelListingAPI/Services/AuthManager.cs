using HotelListingAPI.Model;
using HotelListingAPI.Model.DTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace HotelListingAPI.Services
{
    public class AuthManager : IAuthManager
    {
        private readonly UserManager<ApiUser> _userManager;
        private readonly IConfiguration _configuration;
        private ApiUser _user;

        public AuthManager(UserManager<ApiUser> userManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _configuration = configuration;
        }
        public async Task<string> CreateToken()
        {
            var signingCredentials = GetSigningCredentials();
            var claims = await GetClaims();
            var token = GenerateTokenOptions(signingCredentials, claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
        {
            var jwtSettings = _configuration.GetSection("JWT");
            var expiration = DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings.GetSection("lifetime").Value));

            var token = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: expiration,
                signingCredentials: signingCredentials
                );
            return token;
        }

        private async Task<List<Claim>> GetClaims()
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, _user.UserName
                )
            };
            var roles = await _userManager.GetRolesAsync(_user);

            foreach ( var role in roles )
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }
            return claims;
        }

        private SigningCredentials GetSigningCredentials()
        {
            //var Key = "YOU CAN PUT ANYTHING LIKE YOUR NAME OR ANYTHING AS YOUR KEY"; //_configuration.GetValue<string>("KEY");
            //var Key = Environment.GetEnvironmentVariable("KEY");//if i save my securityKey in the command promt(i.e we use Environment.GetEnvironmentVariable)
            var Key = _configuration.GetSection("JWT");//securityKey saved in appsettings.json(we used this line of code)
            //var keyValue = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Key));//continuation of command prompt savings
            var keyValue = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("JWT:KEY").Value));//saved in appsettings.json
            var signingCredential = new SigningCredentials(keyValue, SecurityAlgorithms.HmacSha256);
            return signingCredential;
            //return new SigningCredentials(keyValue, SecurityAlgorithms.HmacSha256);
        }

        public async Task<bool> ValidateUser(LoginUserDto userDto)
        {
            _user = await _userManager.FindByNameAsync(userDto.Email);
            return (_user != null && await _userManager.CheckPasswordAsync(_user, userDto.Password));
        }
    }
}
