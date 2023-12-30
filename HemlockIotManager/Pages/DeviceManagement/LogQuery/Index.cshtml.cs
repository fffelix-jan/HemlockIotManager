using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task OnGetAsync(long? deviceId, DateTime? startTime, DateTime? endTime)
        {
            var query = _context.LogEntries.AsQueryable();

            if (deviceId.HasValue)
            {
                query = query.Where(le => le.DeviceID == deviceId.Value);
            }

            if (startTime.HasValue && endTime.HasValue)
            {
                query = query.Where(le => le.LogDateTime >= startTime.Value && le.LogDateTime <= endTime.Value);
            }

            LogEntries = await query.ToListAsync();
        }
    }
}
