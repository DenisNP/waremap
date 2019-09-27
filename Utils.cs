using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;
using Waremap.Models;

namespace Waremap
{
    public static class Utils
    {
        public static readonly JsonSerializerSettings ConverterSettings = new JsonSerializerSettings
        {
            ContractResolver = new DefaultContractResolver
            {
                NamingStrategy = new SnakeCaseNamingStrategy(),
            },
            Converters = new List<JsonConverter>
            {
                new StringEnumConverter()
            }
        };

        public static int CreateIdFor(IEnumerable<int> ids)
        {
            return ids.Max() + 1;
        }

        public static int Dist(int x1, int y1, int x2, int y2)
        {
            var dX = x1 - x2;
            var dY = y1 - y2;
            return (int) Math.Round(Math.Sqrt(dX * dX + dY * dY));
        }

        public static int Dist(Node node1, Node node2)
        {
            return Dist(node1.X, node1.Y, node2.X, node2.Y);
        }
    }
}