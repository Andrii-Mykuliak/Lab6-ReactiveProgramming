using Microsoft.AspNetCore.SignalR.Client;
using Newtonsoft.Json;

namespace TelemetryService
{
    public class SpaceTelemetryClient
    {
        private HubConnection hubConnection;
        private Random random = new Random();

        public void InitializeConnection()
        {
            Console.WriteLine("Initializing connection to SignalR Hub...");

            hubConnection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5000/telemetry")
                .Build();

            hubConnection.On<string>("ReceiveMessage", (message) =>
            {
                Console.WriteLine("Received Message: " + message);
            });
        }

        public async Task StartConnectionAsync()
        {
            try
            {
                await hubConnection.StartAsync();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error connecting to hub: {ex.Message}");
            }
        }

        public async Task SendTelemetryDataAsync()
        {
            using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

            Console.WriteLine("Press any key to stop sending messages...");
            while (!Console.KeyAvailable)
            {
                await timer.WaitForNextTickAsync();

                if (random.NextDouble() < 0.7) // 70% probability
                {
                    var telemetryData = GenerateTelemetryData();
                    var telemetryJson = JsonConvert.SerializeObject(telemetryData);
                    await hubConnection.InvokeAsync("ProcessMessage", telemetryJson);
                }
            }
        }

        private ScienceDataDTO GenerateTelemetryData()
        {
            var location = new LocationDTO
            {
                X = random.NextDouble() * 1000 - 500,  // -500 до 500
                Y = random.NextDouble() * 1000 - 500,  // -500 до 500
                Z = random.NextDouble() * 1000 - 500   // -500 до 500
            };

            int objectTypeChoice = random.Next(0, 100);

            if (objectTypeChoice < 25) // Космічний корабель
            {
                return new ScienceDataDTO
                {
                    Location = location,
                    SpectralIntensity = (decimal)(random.NextDouble() * 10 + 70),
                    GravitationalFieldValue = (decimal)(random.NextDouble() * 2 + 8.6),
                    CurrentAltitude = random.NextDouble() * 300
                };
            }
            else if (objectTypeChoice < 50) // Астероїд
            {
                return new ScienceDataDTO
                {
                    Location = location,
                    SpectralIntensity = (decimal)(random.NextDouble() * 40 + 30),
                    GravitationalFieldValue = (decimal)(random.NextDouble() * 2),
                    CurrentAltitude = random.NextDouble() * 3000 + 500
                };
            }
            else if (objectTypeChoice < 75) // Зірка
            {
                return new ScienceDataDTO
                {
                    Location = location,
                    SpectralIntensity = (decimal)(random.NextDouble() * 1000 + 1000),
                    GravitationalFieldValue = (decimal)(random.NextDouble() * 1000 + 100),
                    CurrentAltitude = random.NextDouble() * 50000 + 10000
                };
            }
            else // Неопізнане
            {
                return new ScienceDataDTO
                {
                    Location = location,
                    SpectralIntensity = (decimal)(random.NextDouble() * 100),
                    GravitationalFieldValue = (decimal)(random.NextDouble() * 100),
                    CurrentAltitude = random.NextDouble() * 5000
                };
            }
        }

        public async Task CloseConnectionAsync()
        {
            await hubConnection.DisposeAsync();
        }
    }
}
