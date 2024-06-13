namespace TokenTestingBlazor.Util
{
    public class CanvasProfile
    {
        //Values from appsettings.json
        private readonly string oAuthClientID;
        private readonly string oAuthClientSecret;
        private readonly string redirectURI;
        private readonly string tokenURI;

        private readonly HttpClient client;
        public CanvasProfile(IConfiguration Config)
        {
            client = new HttpClient();
            oAuthClientID = Config["Canvas:client_id"] ?? throw new ArgumentNullException(nameof(oAuthClientID));
            oAuthClientSecret = Config["Canvas:client_secret"] ?? throw new ArgumentNullException(nameof(oAuthClientSecret));
            redirectURI = Config["Canvas:redirect_uri"] ?? throw new ArgumentNullException(nameof(redirectURI));
            tokenURI = Config["Canvas:token_uri"] ?? throw new ArgumentNullException(nameof(tokenURI));
        }
    }
}
