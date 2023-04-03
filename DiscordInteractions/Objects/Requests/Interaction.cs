namespace DiscordInteractions.Objects.Requests
{
    public record Interaction<T>
    {
        public InteractionType Type { get; set; }
        public T? Data { get; set; }
        public string? Json { get; set; }
    }

    public enum InteractionType
    {
        Ping = 1,
        Application_Command = 2,
        Message_Component = 3,
        Application_Command_Autocomplete = 4,
        Modal_Submit = 5
    }
}
