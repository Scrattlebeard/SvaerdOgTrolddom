using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SvaerdOgTroldom.Commands;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SvaerdOgTrolddom.Commands
{
    public class LuckCommand
    {
        private ILogger _log;
        private DiceRoller _roller;        

        public LuckCommand(DiceRoller roller, ILogger log)
        {            
            _log= log;
            _roller = roller;
        }

        public static CommandDefinition Definition
        {
            get =>
            new()
            {
                Name = "prøvlykken",
                Description = "Test dit held",
                Type = ApplicationCommandType.Chat_Input,
                Options = new List<OptionDefinition>()
                {
                    new OptionDefinition
                    {
                        Name = "held",
                        Description = "Spillerens nuværende held",
                        Type = ApplicationCommandOptionType.Integer,
                        Required= true
                    }                    
                }
            };
        }

        public JsonResult Execute(ApplicationCommandData commandData)
        {
            var luck = int.Parse(commandData.Options.FirstOrDefault().Value);
            _log.LogInformation($"Responding to \\proevlykken command with arg {luck}");

            var (res, rolls, _) = _roller.Roll("2d6");

            var msgBuilder = new StringBuilder($"L.A.R.S held er **{luck}**. ");
            msgBuilder.AppendLine($"Han prøver lykken og slår {_roller.SummarizeRolls(rolls)}.");
            msgBuilder.AppendLine(res <= luck ? "L.A.R.S er **heldig**!" : "L.A.R.S er **uheldig**!");
            msgBuilder.AppendLine($"*(L.A.R.S held falder med én og er nu {luck - 1}.)*");

            return new JsonResult(InteractionResponse.WithContent(msgBuilder.ToString()));
        }
    }
}
