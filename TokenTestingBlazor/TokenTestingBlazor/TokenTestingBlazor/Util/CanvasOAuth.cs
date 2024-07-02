using System.Text.Json;
using TokenTestingBlazor.Models;

namespace TokenTestingBlazor.Util
{
    /// <summary>
    /// Utility Class for getting canvas OAuth tokens
    /// <see href="https://canvas.instructure.com/doc/api/file.oauth.html"/>
    /// </summary>
    public class CanvasOAuth
    {
        //Values from appsettings.json
        private readonly string _oAuthClientID;
        private readonly string _oAuthClientSecret;
        private readonly string _redirectURI;
        private readonly string _tokenURI;

        private readonly HttpClient _client;
        public CanvasOAuth(IConfiguration Config)
        {
            _client = new HttpClient();
            _oAuthClientID = Config["Canvas:client_id"] ?? throw new ArgumentNullException(nameof(_oAuthClientID));
            _oAuthClientSecret = Config["Canvas:client_secret"] ?? throw new ArgumentNullException(nameof(_oAuthClientSecret));
            _redirectURI = Config["Canvas:redirect_uri"] ?? throw new ArgumentNullException(nameof(_redirectURI));
            _tokenURI = Config["Canvas:token_uri"] ?? throw new ArgumentNullException(nameof(_tokenURI));
        }

        /// <summary>
        /// Exchanges the Auth code for the Canvas access token
        /// </summary>
        /// <param name="authCode">Authorization Code from canvas</param>
        /// <returns>A DTO containing the Canvas Access Token</returns>
        public async Task<CanvasToken> GetCanvasTokenAsync(string authCode)
        {
            var endpoint = new Uri(_tokenURI);

            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", _oAuthClientID },
                { "client_secret", _oAuthClientSecret },
                { "redirect_uri", _redirectURI },
                { "code", authCode },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await _client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();



            return JsonSerializer.Deserialize<CanvasToken>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Refreshes the Canvas Access Token
        /// </summary>
        /// <param name="refreshToken">Canvas refresh token</param>
        /// <returns>Canvas refresh token</returns>
        public async Task<CanvasRefreshToken> RefreshCanvasTokenAsync(string refreshToken)
        {
            var endpoint = new Uri(_tokenURI);

            var values = new Dictionary<string, string>()
            {
                { "grant_type", "refresh_token" },
                { "client_id", _oAuthClientID },
                { "client_secret", _oAuthClientSecret },
                { "redirect_uri", _redirectURI },
                { "refresh_token", refreshToken },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await _client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<CanvasRefreshToken>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Logs the user out of canvas
        /// </summary>
        /// <param name="AccessToken">User's access token</param>
        /// <returns>A boolean indicating if the user was successfully logged out</returns>
        public async Task<bool> CanvasLogout(string AccessToken)
        {
            var endpoint = _tokenURI + "?access_token=" + AccessToken;

            var response = await _client.DeleteAsync(endpoint);

            return response.IsSuccessStatusCode;
        }

    }
}
