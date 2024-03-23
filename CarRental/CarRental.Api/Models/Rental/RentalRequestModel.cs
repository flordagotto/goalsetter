using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Api.Models.Rental
{
    [JsonObject(Title = "rental_request_model")]
    public class RentalRequestModel
    {
        [Required(ErrorMessage = "client_id is required")]
        [JsonProperty(PropertyName = "client_id")]
        public long ClientId { get; set; }

        [Required(ErrorMessage = "vehicle_id is required")]
        [JsonProperty(PropertyName = "vehicle_id")]
        public long VehicleId { get; set; }

        [Required(ErrorMessage = "start_date is required")]
        [JsonProperty(PropertyName = "start_date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "end_date is required")]
        [JsonProperty(PropertyName = "end_date")]
        public DateTime EndDate { get; set; }
    }
}