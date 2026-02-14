using CSharpChess;

namespace WebServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            await HttpConnection.HttpConnection.StartConnection("http://localhost:54321/");
        }
        public static async Task<string> HandleClient(string request)
        {
            Console.Write(request);
            string json = "{\"message\":\"Hello from C#\"}";
            return json;
        }
    }
}
