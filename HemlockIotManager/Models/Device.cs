using System.ComponentModel.DataAnnotations;

namespace HemlockIotManager.Models
{
    public class Device
    {
        [Key]
        public long DeviceID { get; set; }

        [Required]
        [DisplayColumn("Device Name")]
        public string DeviceName { get; set; }

        public string Description { get; set; }
    }
}
