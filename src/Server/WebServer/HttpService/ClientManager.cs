using System.Net.WebSockets;
using System.Text;

namespace WebServer.HttpService
{
    internal static class ClientManager
    {
        public static Collection<ClientInstance> clients = [];
        public static Collection<WebSocket> buffer = [];

        public static string IdentifyWebsocket(WebSocket ws, string id = "NoID")
        {
            if (id != "NoID")
            {
                foreach (var client in clients)
                {
                    if (client.Id == id)
                    {
                        client.connection = ws;
                        return String.Empty;
                    }
                }
            }
            var newClient = new ClientInstance(ws);
            clients.Add(newClient);
            return newClient.Id;
        }

        public static void UnAssignWebsocket(WebSocket ws)
        {
            foreach (var client in clients)
            {
                if (client.connection == ws)
                {
                    client.connection = null;
                    break;
                }
            }
        }

        public static async Task SendMessageAll(string message)
        {
            foreach (var client in clients)
            {
                await SendMessage(message, client.connection!).ConfigureAwait(false);
            }
        }

        public static async Task SendMessage(string message, WebSocket webSocket)
        {
            if (webSocket != null && webSocket.State == WebSocketState.Open && !string.IsNullOrEmpty(message))
            {
                var buffer = Encoding.UTF8.GetBytes(message);
                await webSocket.SendAsync(
                    new ArraySegment<byte>(buffer),
                    WebSocketMessageType.Text,
                    true,
                    CancellationToken.None).ConfigureAwait(false);
            }
        }
    }
}
