using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace SvaerdOgTroldom.Commands
{
    public class DiceRoller
    {
        private readonly ILogger _log;               

        public DiceRoller(ILogger log)
        {
            _log = log;
        }

        public (int, List<int>, int) Roll(string arg)
        {
            var (n, sides, bonus) = ParseDiceString(arg);
            var rolls = Roll(n, sides);
            return (rolls.Sum() + bonus, rolls, bonus);
        }

        public List<int> Roll(int n, int sides)
        {
            var rolls = new List<int>(n);
            for (var i = 0; i < n; i++)
            {
                var roll = Random.Shared.Next(sides) + 1;                
                rolls.Add(roll);
            }

            return rolls;
        }

        public string SummarizeRolls(List<int> rolls, int bonus = 0)
        {
            var msg =  rolls.Aggregate("(", (s, n) => s.Length == 1 ? s + n : s + " + " + n) + ")";
            var bonusMsg = bonus != 0 ? $" *+ {bonus}*" : string.Empty;
            return $"**{rolls.Sum() + bonus}** {msg}{bonusMsg}";
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
