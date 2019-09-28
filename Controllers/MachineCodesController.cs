using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Waremap.Controllers
{
    [ApiController]
    [Route("/machine")]
    public class MachineCodesController : ControllerBase
    {
        [HttpGet]
        public string Get()
        {
            if (Request.Query.ContainsKey("id") && int.TryParse(Request.Query["id"], out var nodeId))
            {
                var state = ReceiveEventController.GetState();
                var node = state.Geo.Nodes.FirstOrDefault(n => n.Id == nodeId);
                if (node != null)
                {
                    var parts = state.Equipment.Parts.Where(
                        p => p.Path.Any(process => node.OperationIds.Contains(process.Id))
                    );

                    var awaitingParts = parts.Select(p =>
                    {
                        var operation = state.Equipment.Operations.FirstOrDefault(o => o.Id == node.OperationIds.First());
                        return new AwaitingPart
                        {
                            PartId = p.Id,
                            MachineId = nodeId,
                            PartName = p.Name,
                            OperationId = operation?.Id ?? 0,
                            OperationName = operation?.Name ?? ""
                        };
                    });

                    return JsonConvert.SerializeObject(awaitingParts, Utils.ConverterSettings);
                }
            }

            return "No correct id specified";
        }

        private struct AwaitingPart
        {
            public int PartId { get; set; }
            public int MachineId { get; set; }
            public string PartName { get; set; }
            public int OperationId { get; set; }
            public string OperationName { get; set; }
        }
    }
}