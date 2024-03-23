using CarRental.Api.Models.Client;
using CarRental.Api.Models.Rental;
using CarRental.Api.Models.Vehicle;
using CarRental.Tests.Infrastructure;
using FluentAssertions;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using TechTalk.SpecFlow;
using Xunit;

namespace CarRental.Tests.Acceptance
{
    [Binding]
    public class RentalsSteps : IClassFixture<ServerTestFixture>, IDisposable
    {
        private readonly ServerTestFixture _server;
        private readonly HttpClient _client;
        private ClientResponseModel _registeredClient;

        public RentalsSteps(ServerTestFixture server)
        {
            _server = server;
            _client = _server.CreateClient();
        }

        [Given(@"The Client '(.*)' '(.*)' is registered")]
        public async Task GivenTheClientIsRegistered(string name, string lastName)
        {
            var clientRequest = new ClientRequestModel
            {
                Name = name,
                LastName = lastName
            };

            var response = await _client.PostAsJsonAsync("api/clients", clientRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var registeredClient = JsonConvert.DeserializeObject<ClientResponseModel>(await response.Content.ReadAsStringAsync());

            registeredClient.Id.Should().Be(1);
            registeredClient.Name.Should().Be(name);
            registeredClient.Lastname.Should().Be(lastName);

            _registeredClient = registeredClient;
        }

        [Given(@"There is a vehicle '(.*)' with Price (.*)")]
        public async Task GivenThereIsAVehicleWithPrice(string description, Decimal price)
        {
            var vehicleRequest = new VehicleRequestModel()
            {
                Description = description,
                PricePerDay = price
            };

            var response = await _client.PostAsJsonAsync("api/vehicles", vehicleRequest);

            response.StatusCode.Should().Be(HttpStatusCode.Created);
            var registeredVehicle = JsonConvert.DeserializeObject<VehicleResponseModel>(await response.Content.ReadAsStringAsync());

            registeredVehicle.Id.Should().BeGreaterThan(0);
            registeredVehicle.Description.Should().Be(description);
            registeredVehicle.PricePerDay.Should().Be(price);
        }

        [When(@"The client creates a rental from '(.*)' to '(.*)' With vehicle (.*)")]
        public async Task WhenTheClientCreatesARentalFromToWhitVehicle(DateTime startDate, DateTime endDate, long vehicleId)
        {
            var rentalRequest = new RentalRequestModel()
            {
                ClientId = _registeredClient.Id,
                VehicleId = vehicleId,
                StartDate = startDate,
                EndDate = endDate
            };

            var response = await _client.PostAsJsonAsync("api/rentals", rentalRequest);
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            var rentalCreated = JsonConvert.DeserializeObject<RentalResponseModel>(await response.Content.ReadAsStringAsync());

            rentalCreated.Id.Should().Be(1);
            rentalCreated.ClientId.Should().Be(_registeredClient.Id);
            rentalCreated.VehicleId.Should().Be(vehicleId);
            rentalCreated.StartDate.Should().Be(startDate);
            rentalCreated.EndDate.Should().Be(endDate);
        }

        [Then(@"The client has a rental for (.*)")]
        public async Task ThenTheClientHasRentalFor(int price)
        {
            var response = await _client.GetAsync($"api/clients/{_registeredClient.Id}");

            response.StatusCode.Should().Be(HttpStatusCode.OK);
            var client = JsonConvert.DeserializeObject<ClientResponseModel>(await response.Content.ReadAsStringAsync());

            client.Rentals.Should().HaveCount(1);
            client.Rentals.First().Price.Should().Be(price);
        }

        public void Dispose()
        {

        }
    }
}
