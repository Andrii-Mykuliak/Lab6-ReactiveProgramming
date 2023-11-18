namespace TelemetryService
{
    public class ScienceDataDTO
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
    }
}
