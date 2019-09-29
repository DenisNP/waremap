using System.Linq;
using Newtonsoft.Json;

namespace Waremap.Models.Alice
{
    public class ResponseModel
    {
        public string Text { get; set; }

        public string Tts { get; set; }

        public CardModel Card { get; set; }

        public bool EndSession { get; set; }

        public ButtonModel[] Buttons { get; set; }

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
    }
}