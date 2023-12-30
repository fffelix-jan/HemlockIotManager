using HemlockIotManager.Data;
using HemlockIotManager.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace HemlockIotManager.Pages
{
    public class TestDataGeneratorRemoveBeforeDeploymentModel(ApplicationDbContext context) : PageModel
    {
        private readonly ApplicationDbContext _context = context;

        public DateTime GenerateRandomDateTime()
        {
            Random rnd = new Random();
            DateTime start = new DateTime(1999, 1, 1);
            int range = (DateTime.Today - start).Days;
            return start.AddDays(rnd.Next(range)).AddHours(rnd.Next(24)).AddMinutes(rnd.Next(60)).AddSeconds(rnd.Next(60));
        }

        public async Task<IActionResult> OnPostAsync()
        {
            // Clear LogEntry
            _context.LogEntries.RemoveRange(_context.LogEntries);
            await _context.SaveChangesAsync();

            // Generate test data
            Random random = new Random();
            for (int deviceId = 1; deviceId <= 10; deviceId++)
            {
                for (int i = 0; i < 1000; i++)
                {
                    _context.LogEntries.Add(new LogEntry
                    {
                        // LogID is omitted so it's auto-generated by the database
                        DeviceID = deviceId,
                        LogDateTime = GenerateRandomDateTime(),
                        // Generate coordinates within Hangzhou, China boundary
                        Latitude = random.NextDouble() * (30.4 - 30.1) + 30.1,
                        Longitude = random.NextDouble() * (120.3 - 118.0) + 118.0,
                        Altitude = random.NextDouble() * 100, // Example altitude in meters
                        BatteryLevel = (float)random.NextDouble() * 100, // Battery level between 0-100%
                        LogMessage = "Test log entry"
                    });
                }
            }

            // Save the changes in batches to avoid performance issues
            const int batchSize = 100; // Adjust the batch size as needed
            for (int i = 0; i < _context.LogEntries.Local.Count; i += batchSize)
            {
                await _context.SaveChangesAsync();
                _context.ChangeTracker.Clear(); // Clear the change tracker to free memory
            }

            return RedirectToPage("./Index"); // Redirect to a confirmation page or index
        }

    }
}