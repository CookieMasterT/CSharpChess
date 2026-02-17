using CSharpChess;
using Newtonsoft.Json;
using GDP = WebServer.GameDataParsers;

namespace WebServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            GameLogic.SetupBoard();
            await HttpConnection.HttpConnection.StartConnection("http://localhost:54321/");
        }
        public class Request
        {
            public string? infoRequest;
        }
        public static async Task<string> HandleClient(string requestStr)
        {
            Console.Write(requestStr);
            var requestObj = JsonConvert.DeserializeObject<Request>(requestStr);
            string json;
            switch (requestObj?.infoRequest)
            {
                case "boardState":
                    json = GDP.ChessBoard.json;
                    break;
                default:
                    json = string.Empty;
                    break;
            }
            return json;
        }
    }
}
