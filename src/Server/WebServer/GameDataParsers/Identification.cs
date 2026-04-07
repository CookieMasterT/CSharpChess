using Newtonsoft.Json.Linq;

namespace WebServer.GameDataParsers
{
    internal static class Identification
    {
        public static string GetJson(string id)
        {
            return new JObject(new JProperty("Id", id)).ToString();
        }
    }
}
