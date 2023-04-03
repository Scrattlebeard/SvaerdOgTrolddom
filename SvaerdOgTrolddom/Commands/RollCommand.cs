using DiscordInteractions.Objects.Requests;
using DiscordInteractions.Objects.Responses;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SvaerdOgTroldom.Commands
{
    public class RollCommand
    {
        private readonly ILogger _log;

        public List<int> Rolls { get; } = new List<int>();
        public int Bonus { get; set; } = 0;

        public RollCommand(ILogger log)
        {
            _log = log;
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
            Roll(arg.Value);

            return new JsonResult(InteractionResponse.WithContent($"Resultat af {arg.Value}: {SummarizeRolls()}"));
        }

        public int Roll(string arg)
        {
            var (n, sides, bonus) = ParseDiceString(arg);
            return Roll(n, sides, bonus);
        }

        public int Roll(int n, int sides, int bonus = 0)
        {
            var sum = 0;
            for (var i = 0; i < n; i++)
            {
                var roll = Random.Shared.Next(sides) + 1;
                sum += roll;
                Rolls.Add(roll);
            }

            if (bonus != 0)
            {
                Bonus = bonus;
            }

            return sum + bonus;
        }

        public void Reset() { Rolls.Clear(); Bonus = 0; }

        public string SummarizeRolls()
        {
            var msg = Rolls.Count > 1 || Bonus != 0 ? Rolls.Aggregate("(", (s, n) => s.Length == 1 ? s + n : s + " + " + n) + ")" : string.Empty;
            var bonusMsg = Bonus != 0 ? $" *+ {Bonus}*" : string.Empty;
            return $"**{Rolls.Sum() + Bonus}** {msg}{bonusMsg}";
        }

        private (int, int, int) ParseDiceString(string str)
        {
            var pattern = "([1-9][0-9]*)d([1-9][0-9]*)([+-][0-9]*)?";
            var match = Regex.Match(str, pattern);

            if (match.Success)
            {
                _log.LogDebug(match.Groups[0].Value + " G1:" + match.Groups[1].Value + " G2:" + match.Groups[2].Value + " G3:" + match.Groups[3].Value);
                var n = int.Parse(match.Groups[1].Value);
                var sides = int.Parse(match.Groups[2].Value);
                var bonus = match.Groups[3].Success ? int.Parse(match.Groups[3].Value) : 0;

                return (n, sides, bonus);
            }
            else
            {
                throw new ArgumentException(str + " is not a valide dice roll specification.");
            }
        }
    }
}
