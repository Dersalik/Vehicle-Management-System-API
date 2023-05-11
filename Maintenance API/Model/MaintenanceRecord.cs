using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Maintenance_API.Model
{
    public class MaintenanceRecord
    {
        /// <summary>
        /// id of the maintenance record
        /// </summary>
        public int Id { get; set; }

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
        [Column(TypeName = "decimal(18,2)")]

        public decimal Cost { get; set; }
    }
}
