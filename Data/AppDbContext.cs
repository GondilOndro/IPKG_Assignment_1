using Microsoft.EntityFrameworkCore;
using ParkingRoutes.Models;

namespace ParkingRoutes.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<Car> Car { get; set; }
        public DbSet<Parking> Parking { get; set; }
        public DbSet<Route> Route { get; set; }
    }
}
