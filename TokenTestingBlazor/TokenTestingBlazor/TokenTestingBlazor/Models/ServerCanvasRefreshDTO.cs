namespace TokenTestingBlazor.Models
{
    public class ServerCanvasRefreshDTO
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string access_token { get; set; }
        public string token_type { get; set; }
        public int expires_in { get; set; }
        public ServerCanvasUserDTO user { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
