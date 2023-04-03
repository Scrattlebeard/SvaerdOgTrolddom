using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using DiscordInteractions;
using DiscordInteractions.Objects.Requests;
using SvaerdOgTroldom.Commands;

namespace SvaerdOgTroldom
{
    public static class Main
    {
        [FunctionName("Main")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {            
            var content = await req.ReadAsStringAsync();
            log.LogDebug("Received:\n" + content);

            if(!RequestVerifier.VerifySignature(req.Headers, content)) {
                return new UnauthorizedResult();
            }
            log.LogDebug("Verified signature...");

            var interaction = JsonConvert.DeserializeObject<Interaction<object>>(content);
            interaction.Json = content;
            log.LogDebug("Parsed message...");

            if (interaction.Type == InteractionType.Ping)
            {
                log.LogInformation("Responding to ping...");
                return new OkObjectResult(new { Type = 1 });                
            }

            if (interaction.Type == InteractionType.Application_Command)
            {
                var command = JsonConvert.DeserializeObject<Interaction<ApplicationCommandData>>(content);

                switch (command.Data.Name)
                {
                    case "ping": return new PingCommand().Execute();
                    case "rul": return new RollCommand(log).Execute(command.Data);                        
                    case "kæmp": return new FightRoundCommand(new RollCommand(log), log).Execute(command.Data);
                    default: { return new BadRequestResult(); }
                }
            }

            return new BadRequestResult();
        }
    }    
}

