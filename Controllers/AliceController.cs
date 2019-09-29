using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using HatForAlice.Alice;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

using Newtonsoft.Json;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/alice")]
    public class AliceController : ControllerBase
    {
        [HttpPost]
        public string AliceWebhook()
        {
            try
            {
                // read request
                string body;
                using (var reader = new StreamReader(Request.Body)) body = reader.ReadToEnd();
                // Console.WriteLine("http got: " + body);

                var aliceRequest = JsonConvert.DeserializeObject<AliceRequest>(body, Utils.ConverterSettings);
                //var userId = aliceRequest.Session.UserId;

                // ping
                if (aliceRequest.Request.OriginalUtterance == "ping")
                {
                    var pong = new AliceResponse
                    {
                        Response = new ResponseModel
                        {
                            Text = "pong"
                        },
                        Session = aliceRequest.Session
                    };

                    return JsonConvert.SerializeObject(pong, Utils.ConverterSettings);
                }

                // parse request
                Console.WriteLine($"\nRequest got: {body}" );

                // return response
                var aliceResponse = HandleRequest(aliceRequest);
                var stringResponse = JsonConvert.SerializeObject(aliceResponse, Utils.ConverterSettings);

                Console.WriteLine($"\nResponse: {stringResponse}");

                return stringResponse;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return "Error";
            }
        }

        public static AliceResponse HandleRequest(AliceRequest request)
        {
            var response = new AliceResponse { Session = request.Session, Response = new ResponseModel() };
            response.Response.Text = "Извините, произошла ошибка, разработчики уже уведомлены";

            if (request.Request.Command.IsNullOrEmpty() || request.Request.Nlu.Tokens.First() == "да")
            {
                response.Response.Text = "Добро пожаловать в голосовой помощник рабочего. " +
                                         "Я помогу вам добраться до пункта назначения с нужным грузом." +
                                         "Спросите меня, куда идти или попросите переключить на следующую" +
                                         " точку маршрута, если хотите ее пропустить. ";
            }

            if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "куда идти",
                "куда мне",
                "что делать",
                "какая задача",
                "пункт назначения"
            }))
            {
                var node = ReceiveEventController.GetNextNode();
                response.Response.Text = $"Двигайтесь в {node.Name}.";
            }

            if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "следующий пункт",
                "следующая точка",
                "дальше",
                "переключить точку",
                "я на месте"
            }))
            {
                var switchedNode = ReceiveEventController.SwitchToNextNode();
                response.Response.Text = $"Переключаю на точку {switchedNode.Id}. Следуйте в {switchedNode.Name}.";
            }

            if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "где я",
                "где сейчас",
                "где нахожусь",
                "местоположение",
                "локация"
            }))
            {
                var curNode = ReceiveEventController.GetCurrentNode();
                var nextNode = ReceiveEventController.GetNextNode();
                response.Response.Text = $"Вы находитесь в {curNode.Name}. Следуйте в {nextNode.Name}. ";
            }

            return response;
        }
    }
}