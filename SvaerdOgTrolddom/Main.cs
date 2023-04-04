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
using SvaerdOgTrolddom.Commands;
using System.Security;

namespace SvaerdOgTroldom
{
    public static class Main
    {
        [FunctionName("Main")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "get", "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            try
            {
                var interaction = await ParseRequest(req, log);

                if (interaction.Type == InteractionType.Ping)
                {
                    log.LogInformation("Responding to Discord API ping...");
                    return new OkObjectResult(new { Type = 1 });
                }

                if (interaction.Type == InteractionType.Application_Command)
                {
                    var diceRoller = new DiceRoller(log);
                    var command = JsonConvert.DeserializeObject<Interaction<ApplicationCommandData>>(interaction.Json);

                    switch (command.Data.Name)
                    {
                        case "ping": return new PingCommand(log).Execute();
                        case "rul": return new RollCommand(diceRoller, log).Execute(command.Data);
                        case "kæmp": return new FightRoundCommand(diceRoller, log).Execute(command.Data);
                        case "prøvlykken": return new LuckCommand(diceRoller, log).Execute(command.Data);
                        default: { return new BadRequestResult(); }
                    }
                }
            }
            catch (SecurityException)
            {
                return new UnauthorizedResult();
            }

            return new BadRequestResult();
        }

        private static async Task<Interaction<object>> ParseRequest(HttpRequest req, ILogger log)
        {
            var content = await req.ReadAsStringAsync();
            log.LogDebug("Received:\n" + content);

            if (!RequestVerifier.VerifySignature(req.Headers, content))
            {
                throw new SecurityException("Invalid request signature.");
            }
            log.LogDebug("Verified signature...");

            var interaction = JsonConvert.DeserializeObject<Interaction<object>>(content);
            interaction.Json = content;
            log.LogDebug("Parsed message...");

            return interaction;
        }
    }
}

