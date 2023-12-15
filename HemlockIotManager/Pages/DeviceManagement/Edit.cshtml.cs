using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HemlockIotManager.Data;
using HemlockIotManager.Models;

namespace HemlockIotManager.Pages.DeviceManagement
{
    public class EditModel : PageModel
    {
        private readonly HemlockIotManager.Data.ApplicationDbContext _context;

        public EditModel(HemlockIotManager.Data.ApplicationDbContext context)
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

            var device =  await _context.Devices.FirstOrDefaultAsync(m => m.DeviceID == id);
            if (device == null)
            {
                return NotFound();
            }
            Device = device;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Device).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!DeviceExists(Device.DeviceID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool DeviceExists(long id)
        {
            return _context.Devices.Any(e => e.DeviceID == id);
        }
    }
}
