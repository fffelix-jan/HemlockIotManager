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
using Microsoft.AspNetCore.Identity; // Include for IdentityUser
using System.Security.Claims; // Include for Claims

namespace HemlockIotManager.Pages.DeviceManagement
{
    [Authorize]
    public class DetailsModel : PageModel
    {
        private readonly HemlockIotManager.Data.ApplicationDbContext _context;

        public DetailsModel(HemlockIotManager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public Device Device { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FirstOrDefaultAsync(m => m.DeviceID == id);
            if (device == null)
            {
                return NotFound();
            }

            // Get the currently logged-in user's ID
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            // Check if the device's OwnerID matches the logged-in user's ID
            if (device.OwnerID != userId)
            {
                return Forbid(); // Return a 403 Forbidden response
            }

            Device = device;
            return Page();
        }
    }
}
