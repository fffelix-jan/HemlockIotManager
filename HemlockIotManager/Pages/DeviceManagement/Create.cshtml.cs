using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace HemlockIotManager.Pages.DeviceManagement
{
    [Authorize]
    public class CreateModel : PageModel
    {
        private readonly HemlockIotManager.Data.ApplicationDbContext _context;

        public CreateModel(HemlockIotManager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Device Device { get; set; } = default!;

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Device.OwnerID = User.FindFirst(ClaimTypes.NameIdentifier)?.Value ?? string.Empty;

            _context.Devices.Add(Device);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
