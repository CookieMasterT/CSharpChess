using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebServer.HttpService
{
    public static class Connection
    {
        public static async Task StartConnection(string uri)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(uri);
            listener.Start();

            Console.WriteLine($"Listening on {uri}");

            while (true)
            {
                var context = await listener.GetContextAsync();

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
            var wsContext = await context.AcceptWebSocketAsync(null);
            var webSocket = wsContext.WebSocket;

            ClientManager.buffer.Add(webSocket);

            Console.WriteLine("WebSocket connection established.");

            var buffer = new byte[4096];

            while (webSocket.State == WebSocketState.Open)
            {
                var result = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer),
                    CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseAsync(
                        WebSocketCloseStatus.NormalClosure,
                        "Closing",
                        CancellationToken.None);
                    ClientManager.UnAssignWebsocket(webSocket);
                    break;
                }

                var requestBody = Encoding.UTF8.GetString(buffer, 0, result.Count);

                string Response = await Program.HandleClient(requestBody, webSocket);

                await ClientManager.SendMessage(Response, webSocket);
            }
        }
    }
}