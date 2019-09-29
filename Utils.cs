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

        public static int CreateIdFor(List<int> ids)
        {
            return !ids.Any() ? 1 : ids.Max() + 1;
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

        public static bool IsNullOrEmpty(this string s)
        {
            return string.IsNullOrEmpty(s);
        }

        public static bool ContainsStartWith(this IEnumerable<string> list, string start)
        {
            return list.Any(element => element.ToLower().Trim().StartsWith(start) && start.Length >= element.Length / 2);
        }

        public static bool CheckTokens(IEnumerable<string> tokens, params string[] expected)
        {
            return expected.Any(expectedString =>
            {
                var expectedTokens = expectedString.Split(" ");
                return expectedTokens.All(tokens.ContainsStartWith);
            });
        }
    }
}