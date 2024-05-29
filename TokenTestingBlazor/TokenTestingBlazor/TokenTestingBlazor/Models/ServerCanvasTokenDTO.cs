namespace TokenTestingBlazor.Models
{
    /// <summary>
    /// Data Transfer Object for a Canvas Token
    /// </summary>
    public class ServerCanvasTokenDTO
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string canvas_region { get; set; }
        public ServerCanvasUserDTO user { get; set; }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    }
}
