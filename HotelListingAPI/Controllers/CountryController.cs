using AutoMapper;
using HotelListingAPI.IRepository;
using HotelListingAPI.Model;
using HotelListingAPI.Model.DTOs;
using HotelListingAPI.Pagination;
using Marvin.Cache.Headers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.ObjectModel;

namespace HotelListingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountryController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CountryController> _logger;
        private readonly IMapper _mapper;

        public CountryController(IUnitOfWork unitOfWork, ILogger<CountryController> logger, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
            _mapper = mapper;
        }
        [HttpGet]
        //[ResponseCache(Duration = 60)] //when you do caching on API individually
        [ResponseCache(CacheProfileName = "120SecondsDuration")]//Not individually but globally
        //[HttpCacheExpiration(CacheLocation = CacheLocation.Public, MaxAge = 90)]// 1.
        //[HttpCacheValidation(MustRevalidate = true)]// 2. This 1 & 2, will override d global cache configuration i did in ServiceExtension class
        [Route("GetCountries")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountries()
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetAllAsync();
                var results = _mapper.Map<ICollection<CountryDto>>(countries);//Mapping d Db data to DTO data
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        [HttpGet]
        [Route("GetCountriesByPage")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountriesByPage([FromQuery] Paging paging)
        {
            try
            {
                var countries = await _unitOfWork.Countries.GetPageList(paging);
                var results = _mapper.Map<ICollection<CountryDto>>(countries);//Mapping d Db data to DTO data
                return Ok(results);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountries)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        [HttpGet("{id:int}", Name = "GetCountry")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCountry(int id)
        {
            try
            {
            //throw new Exception();//I implimented GlobalErrorHandling here and it work for all d API.
                var country = await _unitOfWork.Countries.Get(q => q.Id == id, new List<string> { "Hotels" });
                var result = _mapper.Map<CountryDto>(country);//Mapping d Db data to DTO data
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Something went wrong in the {nameof(GetCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }

        }
        [HttpPost]
        [Route("CreateCountry")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateCountry([FromBody] CreateCountryDto createCountry)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogError($"Invalid POST attempt in {nameof(CreateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var count = _mapper.Map<Country>(createCountry);
                await _unitOfWork.Countries.insert(count);
                await _unitOfWork.Save();

                return CreatedAtRoute("GetCountry", new { id = count.Id }, createCountry);
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went Wrong While Creating Country in the {nameof(CreateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        //[Authorize]
        [HttpPut]
        [Route("UpdateCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCountry(int id, [FromBody] UpdateCountryDto updateCountry)
        {
            if (!ModelState.IsValid || id < 1)
            {
                _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var editCountry = await _unitOfWork.Countries.Get(c => c.Id == id);
                //var editCountry = await _unitOfWork.Countries.in(c => c.Id == id);
                if (editCountry == null)
                {
                    _logger.LogError($"Invalid UPDATE attempt in {nameof(UpdateCountry)}");
                    return BadRequest("Submitted data is invalide");
                }
                _mapper.Map(updateCountry, editCountry);
                _unitOfWork.Countries.Update(editCountry);
                await _unitOfWork.Save();
                // _unitOfWork.Hotels.insertRange(updateCountry.Hotels);
                
               foreach(var hot in updateCountry.Hotels)
                {
                    var hotel = new Hotel();
                    _mapper.Map(hot, hotel);
                   await _unitOfWork.Hotels.insert(hotel);
                    await _unitOfWork.Save();
                }
               

                return NoContent();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went Wrong While Updating Country in the {nameof(UpdateCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
        }
        [HttpDelete]
        //[HttpGet("{id:int}", Name = "DeleteCountry")]
        [Route("DeleteCountry")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCountry(int id)
        {
            if (id < 1)
            {
                _logger.LogError($"Invali DELETE atempt in {nameof(DeleteCountry)}");
                return BadRequest(ModelState);
            }
            try
            {
                var removeCountry = await _unitOfWork.Countries.Get(c => c.Id == id);
                if (removeCountry == null)
                {
                    _logger.LogError($"Invalid  DELETE attempt in {nameof(DeleteCountry)}");
                    return BadRequest("Submitted data is invalid");
                }
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, $"Something Went Wrong While Deleting Hotel in the {nameof(DeleteCountry)}");
                return StatusCode(500, "Internal Server Error. Please Try Again Later");
            }
            await _unitOfWork.Countries.Delete(id); await _unitOfWork.Save();
            
            return NoContent();
        }
    }
}
