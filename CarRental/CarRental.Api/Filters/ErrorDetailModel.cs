using Newtonsoft.Json;
using System.Collections.Generic;

namespace CarRental.Api.Filters
{
    [JsonObject(Title = "error_detail_model")]
    public class ErrorDetailModel
    {
        public ErrorDetailModel()
        {
            Errors = new List<Error>();
        }
     
        [JsonProperty(PropertyName = "state")]
        public string State { get; set; }

        [JsonProperty(PropertyName = "status_code")]
        public int StatusCode { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }

        [JsonProperty(PropertyName = "errors")]
        public List<Error> Errors { get; set; }
    }

    [JsonObject(Title = "error")]
    public class Error
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "detail")]
        public string Detail { get; set; }
    }
}
