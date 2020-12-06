using System.ComponentModel.DataAnnotations;

namespace ParkingRoutes.Models
{
    public class Route
    {
        public int Id { get; set; }
        [Display(Name = "Departure parking")]
        [Required]
        public int DepartureParkingId { get; set; }
        [Display(Name = "Destination parking")]
        [Required]
        public int DestinationParkingId { get; set; }
        [Display(Name = "Car")]
        [Required]
        public int CarId { get; set; }
        public double Distance { get; set; }
        [Display(Name = "Fuel needed [l]")]
        public double FuelNeeded { get; set; }

        public Car Car { get; set; }
        public Parking DepartureParking { get; set; }
        public Parking DestinationParking { get; set; }
    }
}
