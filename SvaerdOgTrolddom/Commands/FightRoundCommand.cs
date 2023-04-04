using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace SvaerdOgTroldom.Commands
{
    public class FightRoundCommand
    {
        private ILogger _log;
        private DiceRoller _roller;        

        public FightRoundCommand(DiceRoller roller, ILogger log)
        {            
            _log = log;
            _roller = roller;
        }

        public static CommandDefinition Definition
        { get =>
            new()
            {
                Name = "kæmp",
                Description = "Udfører én kamprunde mellem spilleren og modstanderen",
                Type = ApplicationCommandType.Chat_Input,
                Options = new List<OptionDefinition>()
                {
                    new OptionDefinition
                    {
                        Name = "spiller",
                        Description = "Spillerens evne",
                        Type = ApplicationCommandOptionType.Integer,
                        Required= true
                    },
                    new OptionDefinition
                    {
                        Name = "modstander",
                        Description = "Modstanderens evne",
                        Type = ApplicationCommandOptionType.Integer,
                        Required= true
                    }
                }
            };
        }

        public JsonResult Execute(ApplicationCommandData commandData)
        {
            var args = commandData.Options!.Select(o => int.Parse(o.Value)).ToList();
            _log.LogInformation($"Responding to \\kæmp command with args {args[0]} {args[1]}");

            var (playerRes, playerRolls, playerBonus) = _roller.Roll($"2d6+{args[0]}");
            var playerSummary = $"L.A.R.S langer ud efter modstanderen med sit raketdrevne sværd og slår {_roller.SummarizeRolls(playerRolls, playerBonus)}";

            var (enemyRes, enemyRolls, enemyBonus) = _roller.Roll($"2d6+{args[1]}");
            var enemySummary = $"Modstanderen kæmper bravt tilbage med {_roller.SummarizeRolls(enemyRolls, enemyBonus)}";

            var res = playerRes > enemyRes ?
                "L.A.R.S **vinder** og påfører fjenden et drabeligt sår!" :
                (enemyRes > playerRes) ?
                "L.A.R.S **taber** og hans udholdenhed bliver sat på prøve!" :
                "Både L.A.R.S og modstanderen kæmper forgæves om at få overtaget, men det lykkes ikke for nogen af dem.";

            return new JsonResult(InteractionResponse.WithContent(playerSummary + "\n" + enemySummary + "\n" + res));
        }
    }
}
