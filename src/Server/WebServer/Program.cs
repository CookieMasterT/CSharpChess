using CSharpChess;
using Newtonsoft.Json;
using WebServer.RequestTypes;
using GDP = WebServer.GameDataParsers;
using GA = WebServer.GameActions;

namespace WebServer
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            GameLogic.SetupBoard();
            await HttpConnection.HttpConnection.StartConnection("http://localhost:54321/");
        }
        public static async Task<string> HandleClient(string requestStr)
        {
            var requestObj = JsonConvert.DeserializeObject<InitialInfoRequest>(requestStr);
            string json = "{}";
            switch (requestObj?.requestType)
            {
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
