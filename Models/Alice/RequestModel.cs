using System.Linq;
using Newtonsoft.Json.Linq;
using Waremap;

namespace HatForAlice.Alice
{
    public class RequestModel
    {
        public string Command { get; set; }

        public RequestType Type { get; set; }

        public string OriginalUtterance { get; set; }

        public JObject Payload { get; set; }
        
        public NLU Nlu { get; set; }
    }

    public class NLU
    {
        public string[] Tokens { get; set; }
        public string[] Entity { get; set; }

        public string[] GetNonEmptyTokens() => Tokens.Where(x => !x.IsNullOrEmpty()).ToArray();
    }

    public class Tokens
    {
        public int start { get; set; }
        public int end { get; set; }
    }

    public class Entity
    {
        public Tokens tokens { get; set; }
        public string type { get; set; }
        public object value { get; set; }
    }

    //TODO Parse yandex entity value type based on name?
    public enum RequestType
    {
        SimpleUtterance,
        ButtonPressed
    }
}