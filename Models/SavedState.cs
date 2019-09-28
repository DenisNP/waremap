namespace Waremap.Models
{
    public class SavedState
    {
        public State State { get; set; }
        public Background[] Backgrounds { get; set; }
    }

    public class Background
    {
        public int Floor { get; set; }
        public string Base64 { get; set; }
    }
}