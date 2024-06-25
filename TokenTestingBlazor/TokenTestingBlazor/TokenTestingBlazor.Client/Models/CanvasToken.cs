namespace TokenTestingBlazor.Client.Models
{
    public class CanvasToken
    {
        public string access_token { get; set; }
        public string token_type { get; set; }
        public string refresh_token { get; set; }
        public int expires_in { get; set; }
        public string canvas_region { get; set; }
        public CanvasProfile user { get; set; }
    }
}
