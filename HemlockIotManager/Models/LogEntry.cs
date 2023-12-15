using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace HemlockIotManager.Models
{
    public class LogEntry
    {
        [Key]
        public long LogID { get; set; }
    }
}
