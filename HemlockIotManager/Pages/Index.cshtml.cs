using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HemlockIotManager.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public IndexModel(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public int TotalUsers { get; set; }
        public int TotalDevices { get; set; }
        public int TotalLogs { get; set; }
        public Dictionary<string, int> DeviceUsageData { get; set; }

        public async Task OnGetAsync()
        {
            TotalUsers = _userManager.Users.Count();
            TotalDevices = await _context.Devices.CountAsync();
            TotalLogs = await _context.LogEntries.CountAsync();

            // Sample data for device usage
            DeviceUsageData = await _context.LogEntries
                .GroupBy(le => le.LogDateTime.Date)
                .OrderBy(g => g.Key)
                .Select(g => new { Date = g.Key.ToString("yyyy-MM-dd"), Count = g.Count() })
                .ToDictionaryAsync(g => g.Date, g => g.Count);
        }
    }
}
