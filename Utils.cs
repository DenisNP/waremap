using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Waremap
{
    public static class Utils
    {
        public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy()
            },
        };
    }
}