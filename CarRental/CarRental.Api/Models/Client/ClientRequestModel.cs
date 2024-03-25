using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace CarRental.Api.Models.Client
{
    [JsonObject(Title = "client_request_model")]
    public class ClientRequestModel
    {
        [Required(ErrorMessage = "name is required")]
        [StringLength(20)]
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }

        [Required(ErrorMessage = "last_name is required")]
        [StringLength(20)]
        [JsonProperty(PropertyName = "last_name")]
        public string LastName { get; set; }
    }
}