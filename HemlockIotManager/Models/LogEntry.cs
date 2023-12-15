using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace HemlockIotManager.Models
{
    public class LogEntry
    {
        [Key]
        public long LogID { get; set; }

        [ForeignKey("Device")]
        public long DeviceID { get; set; }

        [Required]
        public DateTime LogDateTime { get; set; }

        [Required]
        public double Latitude { get; set; }
        [Required]
        public double Longitude { get; set; }

        [Required]
        public double Altitude { get; set; }

        [Required]
        public float BatteryLevel { get; set; }

        [Required]
        public required string LogMessage { get; set; }
    }
}
