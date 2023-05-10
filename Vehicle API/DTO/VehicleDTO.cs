using System.ComponentModel.DataAnnotations;

namespace Vehicle_API.DTO
{
    public class VehicleDTO
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public DateTime year { get; set; }
        [Required]
        public string VIN { get; set; }
    }
}
