﻿using System.ComponentModel.DataAnnotations;

namespace ParkingRoutes.Models
{
    public class Parking
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }
    }
}
