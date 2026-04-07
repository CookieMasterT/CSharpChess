using System.Net;
using System.Net.WebSockets;
using System.Text;

namespace WebServer.HttpService
{
    internal static class Connection
    {
        public static async Task StartConnection(Uri uri)
        {
            Console.WriteLine($"Listening on {uri}");

            using var listener = new HttpListener();
            listener.Prefixes.Add(uri.ToString());
            listener.Start();

            while (true)
            {
                var context = await listener.GetContextAsync().ConfigureAwait(false);

                if (!context.Request.IsWebSocketRequest)
                {
                    context.Response.StatusCode = 400;
                    context.Response.Close();
                    continue;
                }

                _ = HandleWebSocketConnection(context);
            }
        }

        private static async Task HandleWebSocketConnection(HttpListenerContext context)
        {
            var wsContext = await context.AcceptWebSocketAsync(null).ConfigureAwait(false);
            var webSocket = wsContext.WebSocket;

            ClientManager.buffer.Add(webSocket);

            var buffer = new byte[4096];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None).ConfigureAwait(false);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None).ConfigureAwait(false);
                    ClientManager.UnAssignWebsocket(webSocket);
                    break;
                }

                var requestBody = Encoding.UTF8.GetString(buffer, 0, result.Count);

                string Response = await RequestHandler.HandleClient(requestBody, webSocket).ConfigureAwait(false);

                await ClientManager.SendMessage(Response, webSocket).ConfigureAwait(false);
            }
        }
    }
}