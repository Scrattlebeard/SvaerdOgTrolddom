namespace DiscordInteractions.Objects.Requests
{
    //https://discord.com/developers/docs/interactions/receiving-and-responding#interaction-object-application-command-interaction-data-option-structure
    public record ApplicationCommandInteractionDataOption
    {
        public string Name { get; set; }
        public string? Value { get; set; }

        public ApplicationCommandOptionType Type { get; set; }
        public ApplicationCommandInteractionDataOption[]? Options { get; set; }
        public bool? Focused { get; set; }
    }    
}
