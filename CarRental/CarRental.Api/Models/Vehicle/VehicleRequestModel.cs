using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Api.Models.Vehicle
{
    [JsonObject(Title = "vehicle_request_model")]
    public class VehicleRequestModel
    {
        [Required(ErrorMessage = "description is required")]
        [JsonProperty(PropertyName = "description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "price_per_day is required")]
        [JsonProperty(PropertyName = "price_per_day")]
        public decimal PricePerDay { get; set; }
    }
}