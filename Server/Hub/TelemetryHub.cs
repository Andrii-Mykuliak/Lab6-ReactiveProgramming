using Microsoft.AspNetCore.SignalR;
using Newtonsoft.Json;
using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;

namespace Server
{
    public class TelemetryHub : Hub
    {
        // Create a subject to push telemetry data into
        private static readonly ISubject<TelemetryData> _telemetrySubject = new Subject<TelemetryData>();

        public TelemetryHub()
        {
        }

        static TelemetryHub()
        {
            // Космічний корабель
            _telemetrySubject
                .Where(IsSpacecraft)
                .Subscribe(HandleSpacecraft);

            // Астероїд
            _telemetrySubject
                .Where(IsAsteroid)
                .Subscribe(HandleAsteroid);

            // Зірка
            _telemetrySubject
                .Where(IsStar)
                .Subscribe(HandleStar);

            // Інша умова
            _telemetrySubject
                .Where(IsUnknown)
                .Subscribe(HandleOther);
        }

        private static bool IsStar(TelemetryData data)
        {
            return data.SpectralIntensity > 1000 && data.GravitationalFieldValue > 100M && data.CurrentAltitude > 10000;
        }

        private static bool IsAsteroid(TelemetryData data)
        {
            return data.SpectralIntensity > 30 && data.SpectralIntensity < 70 && data.GravitationalFieldValue < 2.0M && data.CurrentAltitude > 500;
        }

        private static bool IsSpacecraft (TelemetryData data)
        {
            return data.SpectralIntensity > 80 && Math.Abs(data.GravitationalFieldValue - 9.8M) < 1.0M && data.CurrentAltitude < 300;
        }

        private static bool IsUnknown(TelemetryData data) => !IsStar(data) && !IsAsteroid(data) && !IsSpacecraft(data);


        public async Task ProcessMessage(string message)
        {
            var telemetryData = JsonConvert.DeserializeObject<TelemetryData>(message);
            _telemetrySubject.OnNext(telemetryData);
        }

        private static void HandleSpacecraft(TelemetryData data)
        {
            Console.WriteLine($"Another spaceship has been detected {data.Location}");
        }

        private static void HandleAsteroid(TelemetryData data)
        {
            Console.WriteLine($"An asteroid has been detected {data.Location}");
        }

        private static void HandleStar(TelemetryData data)
        {
            Console.WriteLine($"A star has been detecte {data.Location}");
        }

        private static void HandleOther(TelemetryData data)
        {
            Console.WriteLine($"An unidentified object has been detected {data.Location}");
        }
    }


    public class TelemetryData
    {
        public decimal SpectralIntensity { get; set; }
        public decimal GravitationalFieldValue { get; set; }
        public double CurrentAltitude { get; set; }
        public LocationDTO Location { get; set; } = null!;
    }

    public class LocationDTO
    {
        public double X { get; set; }
        public double Y { get; set; }
        public double Z { get; set; }

        public override string ToString()
        {
            return $"X: {X}, Y: {Y}, Z: {Z}";
        }
    }
}

