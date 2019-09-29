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

        public static string GetCorrectNodeName(State state, Node node)
        {
            var newName = "";

            switch (node.Icon)
            {
                case "Node":
                    newName = "Точка"; break;
                case "Machine":
                    newName = $"Участок {state.Equipment.GetOperationById(node.OperationIds.First())}"; break;
                case "Elevator":
                    newName = "Лифт"; break;
                case "Ladder":
                    newName = "Лестница"; break;
                case "Door":
                    newName = "Дверь"; break;
            }
             var place = node.Depot == 0 ? "Коридор" : $"Цех {node.Depot}";

            return $"{newName} {node.Id}, {place}, Этаж {node.Floor}";
        }

        public static AliceResponse HandleRequest(AliceRequest request)
        {
            var response = new AliceResponse { Session = request.Session, Response = new ResponseModel() };
            response.Response.Text = "Извините, произошла ошибка, разработчики уже уведомлены";
            var state = ReceiveEventController.GetState();



            if (request.Request.Command.IsNullOrEmpty() || request.Request.Nlu.Tokens.First() == "да")
            {
                response.Response.Text = "Добро пожаловать в голосовой помощник рабочего. " +
                                         "Я помогу вам добраться до пункта назначения с нужным грузом." +
                                         "Спросите меня, куда идти или попросите переключить на следующую" +
                                         " точку маршрута, если хотите ее пропустить. ";
            }
            else if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "куда идти",
                "куда мне",
                "что делать",
                "какая задача",
                "пункт назначения"
            }))
            {
                
                var node = ReceiveEventController.GetNextNode();


                var name = GetCorrectNodeName(state,node);

                //if (node.Depot == 0)
                //{
                //    node.Name = node.Name.Replace("Цех 0", "Корридор");
                //}

                response.Response.Text = $"Двигайтесь в {name}.";
            } 
            else if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "следующий пункт",
                "следующая точка",
                "дальше",
                "переключить точку",
                "я на месте"
            }))
            {
                var switchedNode = ReceiveEventController.SwitchToNextNode();

                var name = GetCorrectNodeName(state, switchedNode);

                response.Response.Text = $"Переключаю на точку {switchedNode.Id}. Следуйте в {name}.";
            }
            else if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
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

                //if (curNode.Depot == 0)
                //{
                //    curNode.Name = curNode.Name.Replace("Цех 0", "Корридор");
                //}

                //if (nextNode.Depot == 0)
                //{
                //    nextNode.Name = nextNode.Name.Replace("Цех 0", "Корридор");
                //}

                var curName = GetCorrectNodeName(state, curNode);
                var nextName = GetCorrectNodeName(state, nextNode);

                response.Response.Text = $"Вы находитесь в {curName}. Следуйте в {nextName}. ";
            }
            else if (Utils.CheckTokens(request.Request.Nlu.Tokens, new[]
            {
                "как пройти",
                "хочу попасть в",
                "как дойти",
                "как попасть"
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

                    //if (nextNode.Depot == 0)
                    //{
                    //    nextNode.Name = nextNode.Name.Replace("Цех 0", "Корридор");
                    //}

                    var resultPath = ReceiveEventController.FindPath(depot, onCart);


                    var targetName = GetCorrectNodeName(state, resultPath.Item1);
                    var nextName = GetCorrectNodeName(state, nextNode);

                    if (onCart && resultPath.Item2)
                        response.Response.Text = $"Ваш пункт назначения {targetName}. Следуйте в {nextName}. Часть пути придется пройти пешком.";
                    else
                        response.Response.Text = $"Ваш пункт назначения {targetName}. Следуйте в {nextName}.";
                    break;
                }
            }
            return response;
        }
    }
}