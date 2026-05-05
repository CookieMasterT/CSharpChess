#pragma warning disable CA1812, CS8618, CS0649 // Non-nullable field is uninitialized, field is never assigned to, and will always have its default value null
namespace WebServer.RequestTypes
{
    internal class CoordinateInfo
    {
        public int X { get; set; }
        public int Y { get; set; }
    }
}
#pragma warning restore CA1812, CS8618, CS0649
