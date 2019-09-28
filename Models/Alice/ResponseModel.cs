using System.Linq;
using Newtonsoft.Json;

namespace HatForAlice.Alice
{
    public class ResponseModel
    {
        [JsonProperty("text")] public string Text { get; set; }

        [JsonProperty("tts")] public string Tts { get; set; }

        [JsonProperty("card")] public CardModel Card { get; set; }

        [JsonProperty("end_session")] public bool EndSession { get; set; }

        [JsonProperty("buttons")] public ButtonModel[] Buttons { get; set; }

        //public void FromSimple(SimpleResponse firstHelp)
        //{
        //    Text = firstHelp.Text;
        //    if (!firstHelp.Tts.IsNullOrEmpty())
        //    {
        //        Tts = firstHelp.Tts;
        //    }

        //    if (firstHelp.Buttons != null && firstHelp.Buttons.Length > 0)
        //    {
        //        SetButtons(firstHelp.Buttons);
        //    }
        //}

        public void SetButtons(params string[] texts)
        {
            Buttons = texts.Select(b => new ButtonModel(b)).ToArray();
        }

        public void AddButtons(params string[] texts)
        {
            if (Buttons == null)
            {
                SetButtons(texts);
            }
            else
            {
                var concatButtons = texts.Select(b => new ButtonModel(b)).Concat(Buttons);
                Buttons = concatButtons.ToArray();
            }
        }

        //public void AddText(string text)
        //{
        //    Text += text;
        //    if (!Tts.IsNullOrEmpty())
        //    {
        //        Tts += text;
        //    }
        //}

        //public void AddText(SimpleResponse simpleResponse)
        //{
        //    if (!Tts.IsNullOrEmpty())
        //    {
        //        Text += simpleResponse.Text;
        //        Tts += simpleResponse.Tts.IsNullOrEmpty() ? simpleResponse.Text : simpleResponse.Tts;
        //    }
        //    else if (!simpleResponse.Tts.IsNullOrEmpty())
        //    {
        //        Tts = Text + " - " + simpleResponse.Tts;
        //    }
        //    else
        //    {
        //        Text += simpleResponse.Text;
        //    }
        //}
    }
}