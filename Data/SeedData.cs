using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ParkingRoutes.Models;
using System;
using System.Linq;

namespace ParkingRoutes.Data
{
    public static class SeedData
    {
        public static void InitializeDb(IServiceProvider serviceProvider)
        {
            using (var context = new AppDbContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<AppDbContext>>()))
            {
                // Look for any cars in DB.
                if (!context.Car.Any())
                {
                    context.Car.AddRange(
                        new Car
                        {
                            Name = "Volvo V90 D5",
                            AverageConsumption = 5.7D,
                            EngineStartConsumption = 0.04D
                        },
                        new Car
                        {
                            Name = "Saab 9-5 2.3t",
                            AverageConsumption = 9.3D,
                            EngineStartConsumption = 0.08D
                        });
                }

                if (!context.Parking.Any())
                {
                    context.Parking.AddRange(
                        new Parking
                        {
                            Name = "De Brouckère (Brussels)",
                            Latitude = 50.85005284447927D,
                            Longitude = 4.35369258356639D
                        },
                        new Parking
                        {
                            Name = "Center parking (Ghent)",
                            Latitude = 51.05150850877935D,
                            Longitude = 3.724128867678257D,
                        },
                        new Parking
                        {
                            Name = "Fietsenparking Centrum - Zand (Bruges)",
                            Latitude = 51.205160528933234D,
                            Longitude = 3.2197039978180912D
                        });
                }

                context.SaveChanges();
            }
        }
    }
}