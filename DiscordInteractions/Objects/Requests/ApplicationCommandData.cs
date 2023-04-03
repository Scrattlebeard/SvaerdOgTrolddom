namespace DiscordInteractions.Objects.Requests
{
    //https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-application-command-data-structure
    public record ApplicationCommandData
    {
        public string Name { get; set; }
        public ApplicationCommandType Type { get; set; }
        public ApplicationCommandInteractionDataOption[]? Options { get; set; }

    }
}
