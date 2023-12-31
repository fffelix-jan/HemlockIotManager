using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HemlockIotManager.Pages
{
    public class LogEntryModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty(SupportsGet = true)]
        public long? DeviceId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? StartTime { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateTime? EndTime { get; set; }

        public LogEntryModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<LogEntry>? LogEntries { get; set; }

        public async Task<IActionResult> OnGetAsync(long? deviceId, DateTime? startTime, DateTime? endTime)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            var query = _context.LogEntries.AsQueryable();

            if (deviceId.HasValue)
            {
                var device = await _context.Devices.FindAsync(deviceId.Value);
                // Prevent the user from looking at other users' devices
                if (device == null || device.OwnerID.ToString() != currentUserId)
                {
                    return new ForbidResult(); // 403 Forbidden
                }
                query = query.Where(le => le.DeviceID == deviceId.Value);
            }
            else
            {
                return NotFound(); // Or another appropriate result for when deviceId is not provided
            }

            if (startTime.HasValue && endTime.HasValue)
            {
                query = query.Where(le => le.LogDateTime >= startTime.Value && le.LogDateTime <= endTime.Value);
            }

            LogEntries = await query.ToListAsync();

            return Page(); // Return the page for successful operation
        }
    }
}
