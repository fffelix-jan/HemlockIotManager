using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using HemlockIotManager.Data;
using HemlockIotManager.Models;

namespace HemlockIotManager.Pages.DeviceManagement
{
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
            else
            {
                Device = device;
            }
            return Page();
        }
    }
}
