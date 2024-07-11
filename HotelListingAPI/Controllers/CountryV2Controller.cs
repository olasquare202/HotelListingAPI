using AutoMapper;
using HotelListingAPI.Data;
using HotelListingAPI.IRepository;
using HotelListingAPI.Model.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelListingAPI.Controllers
{
    [ApiVersion("2.0", Deprecated = false)]
    [Route("api/{v:apiversion}/country")]
    [ApiController]
    public class CountryV2Controller : ControllerBase
    {
        private readonly DatabaseContext _db;
        private readonly ILogger<CountryV2Controller> _logger;
       

        public CountryV2Controller(DatabaseContext db, ILogger<CountryV2Controller> logger)
        {
            _db = db;
            _logger = logger;
           
        }
        [HttpGet]
        [Route("GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public  IActionResult GetCountries()
        {
            try
            {
              
               return Ok(_db.Countries);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
    }
}
