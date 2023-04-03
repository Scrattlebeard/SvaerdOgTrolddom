using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;

namespace SvaerdOgTroldom.Commands
{
    public class PingCommand
    {
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
            return new JsonResult(InteractionResponse.WithContent("Pong!"));
        }
    }
}
