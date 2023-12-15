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
    public class DeleteModel : PageModel
    {
        private readonly HemlockIotManager.Data.ApplicationDbContext _context;

        public DeleteModel(HemlockIotManager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        [BindProperty]
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

        public async Task<IActionResult> OnPostAsync(long? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var device = await _context.Devices.FindAsync(id);
            if (device != null)
            {
                Device = device;
                _context.Devices.Remove(Device);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
