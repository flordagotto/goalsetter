using CarRental.DataAcces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using System;

namespace CarRental.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(config)
                .CreateLogger();

            try
            {
                Log.Information("Starting App");
                var host = CreateHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;
                    try
                    {
                        using var context = services.GetRequiredService<AppDbContext>();
                        Log.Information("Creating DataBase");
                        context.Database.EnsureCreated();

                        Log.Information("Seeding DataBase");
                        SeedData.Seed(context);
                    }
                    catch (Exception ex)
                    {
                        Log.Error(ex, "An error occurred seeding the DB.");
                    }
                }

                host.Run();
            }
            catch (Exception e)
            {
                Log.Fatal(e, "App start up failed");
            }
            finally
            {
                Log.CloseAndFlush();
            }
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
            .UseSerilog()
            .ConfigureWebHostDefaults(webBuilder =>
            {
                webBuilder.UseStartup<Startup>();
            });
    }
}
