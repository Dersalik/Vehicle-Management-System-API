using System.ComponentModel.DataAnnotations;

namespace Maintenance_API.DTO
{
    public class VehicleDTO
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        public int year { get; set; }
        [Required]
        public string VIN { get; set; }
        public int? StatusCode { get; set; }
    }
}
