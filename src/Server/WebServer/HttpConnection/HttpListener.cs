using System;
using System.Collections.Generic;
using System.Text;
using System.Net;

namespace WebServer.HttpConnection
{
    public static class HttpConnection
    {
        public static async Task StartConnection(string port)
        {
            var listener = new HttpListener();
            listener.Prefixes.Add(port);
            listener.Start();

            Console.WriteLine($"Listening on {port}");

            while (true)
            {
                var context = await listener.GetContextAsync();
                var response = context.Response;

                response.Headers.Add("Access-Control-Allow-Origin", "*");
                response.Headers.Add("Access-Control-Allow-Methods", "GET, POST, OPTIONS");
                response.Headers.Add("Access-Control-Allow-Headers", "Content-Type");

                if (context.Request.HttpMethod == "OPTIONS")
                {
                    response.StatusCode = 200;
                    response.Close();
                    continue;
                }

                string requestBody = "";

                if (context.Request.HasEntityBody)
                {
                    using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
                    {
                        requestBody = await reader.ReadToEndAsync();
                    }
                }

                string Response = await Program.HandleClient(requestBody);
                byte[] buffer = Encoding.UTF8.GetBytes(Response);

                response.ContentType = "application/json";
                response.ContentLength64 = buffer.Length;
                await response.OutputStream.WriteAsync(buffer, 0, buffer.Length);

                response.Close();
            }
        }
    }
}
