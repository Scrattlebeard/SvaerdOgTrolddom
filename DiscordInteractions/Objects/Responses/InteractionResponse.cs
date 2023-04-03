namespace DiscordInteractions.Objects.Responses
{
    public record InteractionResponse
    {
        public int Type { get; set; }
        public InteractionResponseData? Data { get; set; }

        public static InteractionResponse WithContent(string content)
        {
            return new InteractionResponse { Type = 4, Data = new InteractionResponseData { Content = content } };
        }
    }

}
