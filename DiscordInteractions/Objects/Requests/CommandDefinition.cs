namespace DiscordInteractions.Objects.Requests
{
    public record CommandDefinition
    {
        public string Name { get; init; } = "Unknown";
        public ApplicationCommandType Type { get; init; }
        public string Description { get; init; } = "No description";
        public List<OptionDefinition>? Options { get; init; }
    }

    //https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-types
    public enum ApplicationCommandType
    {
        Chat_Input = 1,
        User = 2,
        Message = 3
    }
}
