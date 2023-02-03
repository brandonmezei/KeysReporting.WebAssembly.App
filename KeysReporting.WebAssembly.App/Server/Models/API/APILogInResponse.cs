namespace KeysReporting.WebAssembly.App.Server.Models.API
{
    public class APILogInResponse
    {
        public string? sessionId { get; set; }
        public int? clientId { get; set; }
        public int? userId { get; set; }
        public int? daysUntilPasswordExpires { get; set; }
    }
}
