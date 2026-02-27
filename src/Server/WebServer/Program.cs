using CSharpChess;
using Newtonsoft.Json;
using WebServer.RequestTypes;
using GDP = WebServer.GameDataParsers;
using GA = WebServer.GameActions;
using WebServer.HttpService;
using System.Net.WebSockets;

namespace WebServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Press R to reset the board.");
            Console.WriteLine("Press Esc to exit.");
            GameLogic.SetupBoard();

            _ = HttpService.Connection.StartConnection("http://localhost:54321/");
            while (true)
            {
                var key = Console.ReadKey();
                if (key.Key == ConsoleKey.R)
                {
                    GameLogic.SetupBoard();
                    await ClientManager.SendMessageAll("refreshBoard");
                }
                else if (key.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }
        }
        public static async Task<string> HandleClient(string requestStr, WebSocket ws)
        {
            var requestObj = JsonConvert.DeserializeObject<InitialInfoRequest>(requestStr);
            string json = String.Empty;
            switch (requestObj?.requestType)
            {
                case "identification":
                    var id = ClientManager.IdentifyWebsocket(ws, requestObj.extraInfo ?? "NoID");
                    if (id != string.Empty)
                        json = GDP.Identification.GetJson(id);
                    break;
                case "boardState":
                    json = GDP.ChessBoard.GetJson();
                    break;
                case "pieceMoves":
                    DeserializeInfo<CoordinateInfo>(requestObj.extraInfo, position =>
                    {
                        json = GDP.PieceMoves.GetJson(position);
                    });
                    break;
                case "movePiece":
                    DeserializeInfo<MovePieceParams>(requestObj.extraInfo, moveInfo =>
                    {
                        GA.MovePiece.Execute(moveInfo);
                    });
                    await ClientManager.SendMessageAll("refreshBoard");
                    break;
                case "currentTeam":
                    json = GDP.CurrentTeam.GetJson();
                    break;
            }
            return json;
        }
        private static void DeserializeInfo<T>(string? extraInfo, Action<T> action)
        {
            var data = JsonConvert.DeserializeObject<T>(extraInfo ?? "");
            if (data is null)
                return;

            action(data);
        }
    }
}
