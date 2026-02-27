using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal class Identification
    {
        public static string GetJson(string id)
        {
            return new JObject(new JProperty("Id",id)).ToString();
        }
    }
}
