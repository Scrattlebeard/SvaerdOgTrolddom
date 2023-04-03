namespace DiscordInteractions.Objects.Requests
{
    public record OptionDefinition
    {
        public string Name { get; set; } = "Unknown option";
        public string Description { get; set; } = "No description";
        public ApplicationCommandOptionType Type { get; set; }
        public bool Required { get; set; } = false;
        public List<OptionChoiceDefinition>? Choices { get; set; }

    }

    //https://discord.com/developers/docs/interactions/application-commands#application-command-object-application-command-option-type
    public enum ApplicationCommandOptionType
    {
        Sub_command = 1,
        Sub_command_group = 2,
        String = 3,
        Integer = 4,
        Boolean = 5,
        User = 6,
        Channel = 7,
        Role = 8,
        Mentionable = 9,
        Number = 10,
        Attachment = 11
    }
}
