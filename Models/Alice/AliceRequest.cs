namespace Waremap.Models.Alice
{
    public class AliceRequest
    {
        public MetaModel Meta { get; set; }

        public RequestModel Request { get; set; }

        public SessionModel Session { get; set; }

        public string Version { get; set; }

        public bool HasScreen()
        {
            return Meta?.Interfaces?.Screen != null;
        }
        public bool HasAccountLinking()
        {
            return Meta?.Interfaces?.AccountLinking != null;
        }
        public bool HasPayments()
        {
            return Meta?.Interfaces?.Payments != null;
        }
    }
}
