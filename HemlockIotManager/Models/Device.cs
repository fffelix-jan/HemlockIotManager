using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace HemlockIotManager.Models
{
    public class Device
    {
        [Key]
        [DisplayName("Device ID")]
        public required long DeviceID { get; set; }

        // this is supposed to be a foreign key to the ASP.NET user ID but I couldn't figure out how to do that
        [Required]
        [DisplayName("Owner ID")]
        public required string OwnerID { get; set; }

        [Required]
        [DisplayName("Device Type")]
        public required string DeviceType { get; set; }

        [Required]
        [DisplayName("Is Active")]
        public bool IsActive { get; set; }

        [Required]
        [DisplayName("Device Name")]
        public required string DeviceName { get; set; }

        [Required]
        public required string Description { get; set; }
    }
}
