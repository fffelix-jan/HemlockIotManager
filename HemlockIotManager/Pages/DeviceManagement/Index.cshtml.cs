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
    public class IndexModel : PageModel
    {
        private readonly HemlockIotManager.Data.ApplicationDbContext _context;

        public IndexModel(HemlockIotManager.Data.ApplicationDbContext context)
        {
            _context = context;
        }

        public IList<Device> Device { get;set; } = default!;

        public async Task OnGetAsync()
        {
            Device = await _context.Devices.ToListAsync();
        }
    }
}
