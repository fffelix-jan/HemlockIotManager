using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims; // For accessing user claims

namespace HemlockIotManager.Pages.DeviceManagement
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public IndexModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Device> Device { get; set; } = default!;

        public async Task OnGetAsync()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier); // Get the current user's ID
            Device = await _context.Devices
                                    .Where(d => d.OwnerID == userId) // Filter devices by OwnerID
                                    .ToListAsync();
        }
    }
}
