using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace WebServer.HttpService
{
    internal class ClientManager
    {
        public static List<ClientInstance> clients = new List<ClientInstance>();
        public static List<WebSocket> buffer = new List<WebSocket>();

        public static string IdentifyWebsocket(WebSocket ws, string id = "NoID")
        {
            if (id == "NoID")
            {
                var newClient = new ClientInstance(ws);
                clients.Add(newClient);
                return newClient.Id;
            }
            else
            {
                foreach (var client in clients)
                {
                    if (client.Id == id)
                    {
                        client.connection = ws;
                    }
                }
            }
            return String.Empty;
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
                await SendMessage(message, client.connection!);
            }
        }

        public static async Task SendMessage(string message, WebSocket webSocket)
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
