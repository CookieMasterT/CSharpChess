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

                // todo: refactor this to not be duplicated code
                case "pieceMoves":
                    var position = JsonConvert.DeserializeObject<CoordinateInfo>(requestObj.extraInfo ?? "");
                    if (position is null)
                        break;
                    json = GDP.PieceMoves.GetJson(position);
                    break;
                case "movePiece":
                    var moveInfo = JsonConvert.DeserializeObject<MovePieceParams>(requestObj.extraInfo ?? "");
                    if (moveInfo is null)
                        break;
                    GA.MovePiece.Execute(moveInfo);
                    break;
            }
            return json;
        }
    }
}
