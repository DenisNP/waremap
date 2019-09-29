namespace Waremap.Models.Alice
{
    public class AliceResponse
    {
        public ResponseModel Response { get; set; }

        public SessionModel Session { get; set; }

        public string Version { get; set; } = "1.0";

        public static AliceResponse GenerateDefaultError(AliceRequest request)
        {
            var response = new AliceResponse
            {
                Session = request.Session,
                Response = new ResponseModel { Text = "Произошла ошибка. Разработчик уже уведомлён." }
            };
            return response;
        }
    }
}