using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

using Newtonsoft.Json;
using Waremap.Models;
using Waremap.Models.Alice;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/alice")]
    public class AliceController : ControllerBase
    {
        [HttpPost]
        public Task AliceWebhook()
        {
            Console.WriteLine("Alice request got");
            try
            {
                // read request
                string body;
                using (var reader = new StreamReader(Request.Body)) body = reader.ReadToEnd();
                // Console.WriteLine("http got: " + body);

                var aliceRequest = JsonConvert.DeserializeObject<AliceRequest>(body, Utils.ConverterSettings);
                Response.Headers.Add("Content-Type", "application/json");
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

                    return Response.WriteAsync(JsonConvert.SerializeObject(pong, Utils.ConverterSettings));
                }

                // parse request
                Console.WriteLine($"\nRequest got: {body}" );

                // return response
                var aliceResponse = HandleRequest(aliceRequest);
                var stringResponse = JsonConvert.SerializeObject(aliceResponse, Utils.ConverterSettings);

                Console.WriteLine($"\nResponse: {stringResponse}");

                return Response.WriteAsync(stringResponse);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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

            if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "как пройти",
                "хочу попасть в",
                "как дойти"
            }))
            {
                response.Response.Text = $"Ошибка обнаружения пути";

                var depotNum = request.Request.Nlu.Tokens.Select(x =>
                {
                    if (int.TryParse(x, out var n))
                        return n;
                    
                    return -1;
                }).FirstOrDefault(y => y != -1);

                bool onCart = request.Request.Nlu.Tokens.ContainsStartWith("тележк") ||
                              request.Request.Nlu.Tokens.ContainsStartWith("погруз") ||
                              request.Request.Nlu.Tokens.ContainsStartWith("телег");

                foreach (var depot in ReceiveEventController.GetState().Geo.Depots.Where(depot => depot.Id == depotNum))
                {
                    var nextNode = ReceiveEventController.GetNextNode();
                    var resultPath = ReceiveEventController.FindPath(depot, onCart);

                    if (onCart && resultPath.Item2)
                        response.Response.Text = $"Ваш пункт назначения {resultPath.Item1.Name}. Следуйте в {nextNode.Name}. Часть пути придется пройти пешком.";
                    else
                        response.Response.Text = $"Ваш пункт назначения {resultPath.Item1.Name}. Следуйте в {nextNode.Name}.";
                    break;
                }
            }
            return response;
        }
    }
}