using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using Newtonsoft.Json;
using HemlockIotManager.Models;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HemlockIotManager.Data;
using Microsoft.EntityFrameworkCore;

namespace HemlockIotManager.Services
{
    public class MqttListenerService : BackgroundService
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttOptions;

        public MqttListenerService(IServiceScopeFactory scopeFactory)
        {
            _scopeFactory = scopeFactory;
            InitializeMqttClient();
        }

        private void InitializeMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer(Environment.GetEnvironmentVariable("MQTTHOST"), Convert.ToInt32(Environment.GetEnvironmentVariable("MQTTPORT")))
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to MQTT broker.");
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("gps/coordinates").Build());
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClient.ConnectAsync(_mqttOptions, stoppingToken);
            _mqttClient.UseApplicationMessageReceivedHandler(async e =>
            {
                using (var scope = _scopeFactory.CreateScope())
                {
                    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    try
                    {
                        var payload = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                        var logData = JsonConvert.DeserializeObject<LogData>(payload);
                        if (logData == null) return;

                        var device = await dbContext.Devices.FirstOrDefaultAsync(d => d.DeviceID == logData.DeviceID && d.OwnerID == logData.OwnerID);
                        if (device == null) return;

                        var newLogEntry = new LogEntry
                        {
                            DeviceID = logData.DeviceID,
                            LogDateTime = logData.LogDateTime,
                            Latitude = logData.Latitude,
                            Longitude = logData.Longitude,
                            Altitude = logData.Altitude,
                            BatteryLevel = logData.BatteryLevel,
                            LogMessage = logData.LogMessage
                        };

                        await dbContext.LogEntries.AddAsync(newLogEntry);
                        await dbContext.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error processing message: {ex.Message}");
                    }
                }
            });
        }

        private class LogData
        {
            public long DeviceID { get; set; }
            public string? OwnerID { get; set; }
            public DateTime LogDateTime { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }
            public double Altitude { get; set; }
            public float BatteryLevel { get; set; }
            public string? LogMessage { get; set; }
        }
    }
}
