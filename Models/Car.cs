using System.ComponentModel.DataAnnotations;

namespace ParkingRoutes.Models
{
    public class Car
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        [Display(Name = "Engine start consumption [l]")]
        [Range(0.001, 100.00)]
        public double EngineStartConsumption { get; set; }
        [Required]
        [Display(Name = "Average consumption [l/100km]")]
        [Range(0.001, 100.00)]
        public double AverageConsumption { get; set; }
    }
}
