using System.Collections.Generic;

namespace Waremap.Models.Alice
{
    public class CardModel
    {
        public string Type = "BigImage"; 
        public string ImageId { get; set; }
        public string Title = "";
        public string Description = "";
        public Dictionary<string, string> Button = null;
    }
}