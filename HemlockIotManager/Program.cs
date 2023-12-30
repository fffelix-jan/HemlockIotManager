using HemlockIotManager.Data;
using HemlockIotManager.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using MQTTnet.AspNetCore.Extensions;
using MQTTnet.Server;
using System.Net;

namespace HemlockIotManager
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = Environment.GetEnvironmentVariable("SQLSERVERCONNECTIONSTRING") ?? throw new InvalidOperationException("Connection string not found. Please set the environment variable 'SQLSERVERCONNECTIONSTRING'.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddRazorPages();
            builder.Services.AddTransient<IEmailSender, EmailSender>();

            // Add test MQTT listener service
            builder.Services.AddHostedService<MqttListenerService>();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapRazorPages();

            // Start the MQTT broker
            var mqttServer = app.Services.GetService<IMqttServer>();


            app.Run();
        }
    }
}
