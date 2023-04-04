using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace SvaerdOgTroldom.Commands
{
    public class PingCommand
    {
        private readonly ILogger _log;
        public PingCommand(ILogger logger)
        {
            _log = logger;
        }

        public static CommandDefinition Definition
        { get =>
            new ()
            {
                Name = "ping",
                Description = "Check om botten er i live",
                Type = ApplicationCommandType.Chat_Input
            };
        }

        public JsonResult Execute()
        {
            _log.LogInformation("Responding to \\ping command...");
            return new JsonResult(InteractionResponse.WithContent("Pong!"));
        }
    }
}
