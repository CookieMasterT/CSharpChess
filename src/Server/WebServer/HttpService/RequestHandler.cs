using Newtonsoft.Json;
using System.Net.WebSockets;
using WebServer.RequestTypes;
using GA = WebServer.GameActions;
using GDP = WebServer.GameDataParsers;

namespace WebServer.HttpService
{
    internal static class RequestHandler
    {
        public static async Task<string> HandleClient(string requestStr, WebSocket ws)
        {
            var requestObj = JsonConvert.DeserializeObject<InitialInfoRequest>(requestStr);
            string json = String.Empty;
            switch (requestObj?.RequestType)
            {
                case "identification":
                    var id = ClientManager.IdentifyWebsocket(ws, requestObj.ExtraInfo ?? "NoID");
                    if (!string.IsNullOrEmpty(id))
                        json = GDP.Identification.GetJson(id);
                    break;
                case "boardState":
                    json = GDP.ChessBoard.GetJson();
                    break;
                case "pieceMoves":
                    DeserializeInfo<CoordinateInfo>(requestObj.ExtraInfo, position => json = GDP.PieceMoves.GetJson(position));
                    break;
                case "movePiece":
                    DeserializeInfo<MovePieceParams>(requestObj.ExtraInfo, moveInfo => GA.MovePiece.Execute(moveInfo));
                    await ClientManager.SendMessageAll("refreshBoard").ConfigureAwait(false);
                    break;
                case "movePieceWithPromotion":
                    DeserializeInfo<MovePieceWithPromotionParams>(requestObj.ExtraInfo, moveInfo => GA.MovePieceWithPromotion.Execute(moveInfo));
                    await ClientManager.SendMessageAll("refreshBoard").ConfigureAwait(false);
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