using AutoMapper;
using CarRental.Api.Models.Client;
using CarRental.Api.Models.Rental;
using CarRental.Api.Models.Vehicle;
using CarRental.Domain;
using CarRental.Services.Clients;
using CarRental.Services.Rentals;
using CarRental.Services.Vehicles;

namespace CarRental.Api.AutoMapper
{
    public class MapperProfile : Profile
    {

        public MapperProfile()
        {
            CreateMap<ClientRequestModel, ClientDto>();
            CreateMap<ClientDto, ClientResponseModel>();
            CreateMap<ClientDto, Client>().ReverseMap();

            CreateMap<VehicleRequestModel, VehicleDto>();
            CreateMap<VehicleDto, VehicleResponseModel>();
            CreateMap<VehicleDto, Vehicle>().ReverseMap();

            CreateMap<RentalRequestModel, RentalDto>();
            CreateMap<RentalDto, RentalResponseModel>();
            CreateMap<RentalDto, Rental>().ReverseMap();

        }
    }
}
