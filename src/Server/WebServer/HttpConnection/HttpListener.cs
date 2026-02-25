using System;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace WebServer.HttpConnection
{
    public static class Listener
    {
        static WebSocketContext? wsContext;
        static WebSocket? webSocket;

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
            wsContext = await context.AcceptWebSocketAsync(null);
            webSocket = wsContext.WebSocket;

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
                    break;
                }

                var requestBody = Encoding.UTF8.GetString(buffer, 0, result.Count);

                string Response = await Program.HandleClient(requestBody);

                SendMessage(Response);
            }
        }

        public static async Task SendMessage(string message)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open && message != String.Empty)
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None);
            }
        }
    }
}