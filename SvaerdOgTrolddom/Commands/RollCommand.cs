using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace SvaerdOgTroldom.Commands
{
    public class RollCommand
    {
        private readonly ILogger _log;
        private readonly DiceRoller _roller;

        public RollCommand(DiceRoller roller, ILogger log)
        {
            _log = log;
            _roller = roller;
        }

        public static CommandDefinition Definition
        {
            get =>
             new()
             {
                 Name = "rul",
                 Description = "Slå et antal terninger med det angivne antal sider og en bonus, f.eks. 3d12+7. Default: 1d6,",
                 Type = ApplicationCommandType.Chat_Input,
                 Options = new List<OptionDefinition>() { new OptionDefinition {
                        Name = "terninger",
                        Description = "De terninger der skal slås og den bonus der anvendes, f.eks. 3d12+2 eller 7d42-4. Default: 1d6",
                        Required= false,
                        Type = ApplicationCommandOptionType.String
                    }
                }
             };
        }

        public JsonResult Execute(ApplicationCommandData commandData)
        {            
            var arg = commandData.Options?.FirstOrDefault() ?? new ApplicationCommandInteractionDataOption { Value = "1d6" };
            _log.LogInformation($"Responding to \\rul command with arg {arg}");
            var (res, rolls, bonus) = _roller.Roll(arg.Value);

            return new JsonResult(InteractionResponse.WithContent($"Resultat af {arg.Value}: {_roller.SummarizeRolls(rolls, bonus)}"));
        }        
    }
}
