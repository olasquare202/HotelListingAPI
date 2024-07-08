namespace HotelListingAPI.Model.DTOs
{
    public class UpdateCountryDto : CreateCountryDto
    {
        public ICollection<CreateHotelDto> Hotels { get; set; }
    }
}
