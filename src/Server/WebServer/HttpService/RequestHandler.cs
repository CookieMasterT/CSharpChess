using Newtonsoft.Json;
using System.Net.WebSockets;
using WebServer.RequestTypes;
using GA = WebServer.GameActions;
using GDP = WebServer.GameDataParsers;

namespace WebServer.HttpService
{
    internal class RequestHandler
    {
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