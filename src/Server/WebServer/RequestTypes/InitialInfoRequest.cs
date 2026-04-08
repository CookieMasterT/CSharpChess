namespace WebServer.RequestTypes
{
    internal sealed class InitialInfoRequest
    {
        public string? RequestType { get; set; }
        public string? ExtraInfo { get; set; }
    }
}
