using System.ComponentModel.DataAnnotations;
using Vehicle_API.Validation_attribute;

namespace Vehicle_API.DTO
{
    public class VehicleDTO
    {
        /// <summary>
        /// Make of the vehicle (e.g. Toyota, Honda, etc.)
        /// </summary>
        [Required]
        public string Make { get; set; }
        /// <summary>
        /// model of the vehicle (e.g. Camry, Civic, etc.)
        /// </summary>
        [Required]
        public string Model { get; set; }
        /// <summary>
        /// year of the vehicle (e.g. 2018, 2019, etc.)
        /// </summary>
        [Required]
        [YearRangeAttribute]
        
        public int year { get; set; }
        /// <summary>
        /// VIN of the vehicle (e.g. 1HGCM82633A004352, 1HGCM82633A004353, etc.)
        /// </summary>
        [Required]
        public string VIN { get; set; }
    }
}
