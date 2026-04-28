#pragma warning disable CS8618, CS0649 // Non-nullable field is uninitialized, field is never assigned to, and will always have its default value null
namespace WebServer.RequestTypes
{
    internal sealed class InitialInfoRequest
    {
        public string? RequestType { get; set; }
        public string? ExtraInfo { get; set; }
    }
}
#pragma warning restore CS8618, CS0649
