using System.ComponentModel.DataAnnotations;
using Vehicle_API.Validation_attribute;

namespace Vehicle_API.Model
{
    public class Vehicle
    {
        public int Id { get; set; }
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
