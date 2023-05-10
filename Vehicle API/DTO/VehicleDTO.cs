using System.ComponentModel.DataAnnotations;
using Vehicle_API.Validation_attribute;

namespace Vehicle_API.DTO
{
    public class VehicleDTO
    {
        [Required]
        public string Make { get; set; }
        [Required]
        public string Model { get; set; }
        [Required]
        [YearRangeAttribute]
        public int year { get; set; }
        [Required]
        public string VIN { get; set; }
    }
}
