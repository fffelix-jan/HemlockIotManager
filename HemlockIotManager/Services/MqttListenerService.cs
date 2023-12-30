using Microsoft.AspNetCore.Identity.UI.Services;
using MQTTnet;
using MQTTnet.Client;
using MQTTnet.Client.Options;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HemlockIotManager.Services
{
    public class MqttListenerService : BackgroundService
    {
        private readonly IEmailSender _emailSender;
        private IMqttClient _mqttClient;
        private IMqttClientOptions _mqttOptions;

        public MqttListenerService(IEmailSender emailSender)
        {
            _emailSender = emailSender;
            InitializeMqttClient();
        }

        private void InitializeMqttClient()
        {
            var factory = new MqttFactory();
            _mqttClient = factory.CreateMqttClient();
            _mqttOptions = new MqttClientOptionsBuilder()
                .WithTcpServer("host.docker.internal", 1883) // Assuming the broker is running locally
                .Build();

            _mqttClient.UseConnectedHandler(async e =>
            {
                Console.WriteLine("Connected to MQTT broker.");
                await _mqttClient.SubscribeAsync(new MqttTopicFilterBuilder().WithTopic("gps/coordinates").Build());
            });

            _mqttClient.UseApplicationMessageReceivedHandler(e =>
            {
                var message = Encoding.UTF8.GetString(e.ApplicationMessage.Payload);
                Console.WriteLine($"Received message: {message}");
                SendEmailAsync(message).Wait();
            });
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _mqttClient.ConnectAsync(_mqttOptions, stoppingToken);
        }

        private async Task SendEmailAsync(string message)
        {
            try
            {
                string subject = "New GPS Coordinates";
                string emailBody = $"Received GPS coordinates: {message}";
                await _emailSender.SendEmailAsync("felix@felixan.ca", subject, emailBody);
                Console.WriteLine("Email sent successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error sending email: {ex.Message}");
            }
        }
    }
}
