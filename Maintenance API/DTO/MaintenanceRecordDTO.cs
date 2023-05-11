using System.ComponentModel.DataAnnotations;

namespace Maintenance_API.DTO
{
    public class MaintenanceRecordDTO
    {
        /// <summary>
        /// Id of the vehicle record
        /// </summary>
        [Required]
        public int VehicleId { get; set; }
        /// <summary>
        /// type of the service (e.g. oil change, tire rotation, etc.)
        /// </summary>
        [Required]
        public string ServiceType { get; set; }
        /// <summary>
        /// date of the service
        /// </summary>
        [Required]
        public DateTime Date { get; set; }
        /// <summary>
        /// cost of the service
        /// </summary>
        [Required]
        public decimal Cost { get; set; }
    }
}
