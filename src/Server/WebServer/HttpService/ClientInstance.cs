using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;

namespace WebServer.HttpService
{
    internal class ClientInstance
    {
        public ClientInstance()
        {
            this._id = Guid.NewGuid().ToString();
        }
        public ClientInstance(WebSocket connection) : this()
        {
            this.connection = connection;
        }

        public string _id;
        public string Id { get => _id; }
        public WebSocket? connection;
    }
}
