using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Azure.WebJobs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Net.Http.Json;
using SvaerdOgTroldom.Commands;
using System;
using System.Text.Json;
using DiscordInteractions.Serialization;

namespace SvaerdOgTroldom
{
    public static class RegisterCommands
    {
        [FunctionName("RegisterCommands")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Function, "post", Route = null)] HttpRequest req,
            ILogger log)
        {
            log.LogDebug("RegisterCommands triggered...");

            var appId = 1091808149863944223;
            var guildId = 705525564315926598;
            var token = Environment.GetEnvironmentVariable("BotToken", EnvironmentVariableTarget.Process);
            var globalUrl = $"https://discord.com/api/v10/applications/{appId}/commands";
            var guildUrl = $"https://discord.com/api/v10/applications/{appId}/guilds/{guildId}/commands";
            var content = new[]
            {
                RollCommand.Definition,
                FightRoundCommand.Definition,
                PingCommand.Definition                

            };

            var serializerOptions = new JsonSerializerOptions { PropertyNamingPolicy = new LowerCaseNamingPolicy() };

            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", "Bot " + token);
            var resp = await httpClient.PutAsJsonAsync(globalUrl, content, serializerOptions);

            log.LogInformation(await resp.Content.ReadAsStringAsync());

            await httpClient.PutAsJsonAsync(guildUrl, content, serializerOptions);

            log.LogDebug("RegisterCommands finished.");

            return resp.IsSuccessStatusCode ? new OkResult() : new StatusCodeResult((int)resp.StatusCode);
        }
    }
}
