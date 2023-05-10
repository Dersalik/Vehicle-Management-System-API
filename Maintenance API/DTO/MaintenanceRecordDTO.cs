using System.ComponentModel.DataAnnotations;

namespace Maintenance_API.DTO
{
    public class MaintenanceRecordDTO
    {
        [Required]
        public int VehicleId { get; set; }

        [Required]
        public string ServiceType { get; set; }

        [Required]
        public DateTime Date { get; set; }

        [Required]
        public decimal Cost { get; set; }
    }
}
