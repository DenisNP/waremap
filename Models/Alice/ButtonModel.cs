using Newtonsoft.Json;

namespace HatForAlice.Alice
{
    public class ButtonModel
    {
        public string Title { get; set; }

        public object Payload { get; set; }

        public string Url { get; set; }

        public bool Hide = true;

        public ButtonModel()
        {
            
        }

        public ButtonModel(string t)
        {
            Title = t;
        }
    }
}