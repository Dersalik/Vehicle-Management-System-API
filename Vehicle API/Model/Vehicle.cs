using System.ComponentModel.DataAnnotations;

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
        public DateTime year { get; set; }
        [Required]
        public string VIN { get; set; }
    }
}
