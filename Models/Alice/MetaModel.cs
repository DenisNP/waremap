using System.Collections.Generic;

namespace Waremap.Models.Alice
{
    public class MetaModel
    {
        public string Locale { get; set; }

        public string Timezone { get; set; }

        public string ClientId { get; set; }

        public Interface Interfaces { get; set; }
    }

    public class Interface
    {
        public Dictionary<string, object> Screen { get; set; }
        public Dictionary<string, object> AccountLinking { get; set; }
        public Dictionary<string, object> Payments { get; set; }
    }
}