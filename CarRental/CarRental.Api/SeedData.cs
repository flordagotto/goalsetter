using CarRental.DataAcces;
using CarRental.Domain;
using System;
using System.Linq;

namespace CarRental.Api
{
    public static class SeedData
    {
        public static void Seed(AppDbContext dbContext)
        {
            if (dbContext.Clients.Any())
                return;

            dbContext.Clients.AddRange
                (
                new Client { Name = "Andres", LastName = "Jimenez" },
                new Client { Name = "Ivan", LastName = "Martinez" },
                new Client { Name = "Juan", LastName = "Ramirez" }
                );

            dbContext.SaveChanges();

            dbContext.Vehicles.AddRange
                (
                    new Vehicle { Description = "Ford Fiesta", PricePerDay = 20 },
                    new Vehicle { Description = "Ford Focus", PricePerDay = 25 },
                    new Vehicle { Description = "Ford Mustang", PricePerDay = 50 },
                    new Vehicle { Description = "VW Golf", PricePerDay = 25 }
                );

            dbContext.SaveChanges();

            dbContext.Rentals.AddRange
                (
                    new Rental
                    {
                        ClientId = 1,
                        VehicleId = 2,
                        StartDate = new DateTime(2024, 04, 01),
                        EndDate = new DateTime(2024, 04, 10),
                        Price = 225
                    },
                    new Rental
                    {
                        ClientId = 2,
                        VehicleId = 1,
                        StartDate = new DateTime(2024, 04, 05),
                        EndDate = new DateTime(2024, 04, 10),
                        Price = 250
                    },
                    new Rental
                    {
                        ClientId = 1,
                        VehicleId = 2,
                        StartDate = new DateTime(2024, 05, 01),
                        EndDate = new DateTime(2024, 05, 10),
                        Price = 225
                    },
                    new Rental
                    {
                        ClientId = 3,
                        VehicleId = 4,
                        StartDate = new DateTime(2024, 04, 05),
                        EndDate = new DateTime(2024, 04, 15),
                        Price = 250
                    }
                );

            dbContext.SaveChanges();
        }
    }
}
