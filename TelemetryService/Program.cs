using TelemetryService;

class Program
{
    static async Task Main(string[] args)
    {
        var telemetryClient = new SpaceTelemetryClient();

        telemetryClient.InitializeConnection();
        await telemetryClient.StartConnectionAsync();
        await telemetryClient.SendTelemetryDataAsync();
        await telemetryClient.CloseConnectionAsync();
    }
}