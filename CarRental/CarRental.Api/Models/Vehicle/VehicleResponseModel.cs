namespace CarRental.Api.Models.Vehicle
{
    public class VehicleResponseModel
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public decimal PricePerDay { get; set; }
    }
}