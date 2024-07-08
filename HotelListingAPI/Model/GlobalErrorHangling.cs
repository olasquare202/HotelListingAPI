using Newtonsoft.Json;

namespace HotelListingAPI.Model
{
    public class GlobalErrorHangling
    {
        public int statusCode {  get; set; }
        public string message { get; set; }
        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
