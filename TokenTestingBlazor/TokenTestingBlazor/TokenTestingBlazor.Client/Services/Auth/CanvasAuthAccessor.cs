using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System.Text.Json;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    /// <summary>
    /// Class to access the server side API for canvas authentication
    /// </summary>
    public class CanvasAuthAccessor
    {
        private HttpClient _client;
        private readonly string authEndpoint;
        private readonly string domain;
        public CanvasAuthAccessor(IConfiguration Config) 
        {
            _client = new HttpClient();
            authEndpoint = Config["Canvas:auth_uri"] ?? throw new ArgumentNullException(nameof(authEndpoint));
            domain = Config["Domain"] ?? throw new ArgumentNullException($"{nameof(domain)}");
        }

        /// <summary>
        /// Retrives the access token given an authorization code using the serverside API
        /// </summary>
        /// <param name="AuthCode">Canvas authentication code</param>
        /// <returns>The canvas access token</returns>
        public async Task<CanvasToken> GetAccessTokenAsync(string AuthCode)
        {
            var apiEndpoint = domain + "/api/auth/getToken?code=" + AuthCode;

            _client.DefaultRequestHeaders.Clear();

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<CanvasToken>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Refreshes a Canvas Token using the serverside API
        /// </summary>
        /// <param name="RefreshToken">Canvas Refresh Token</param>
        /// <returns>The refreshed Access Token</returns>
        public async Task<CanvasToken> RefreshAccessTokenAsync(string RefreshToken)
        {
            var apiEndpoint = domain + "/api/auth/refreshToken";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("refresh_token", RefreshToken);

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<CanvasToken>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Logs a user out of the canvas session
        /// </summary>
        /// <param name="AccessToken">User's canvas Access Token</param>
        /// <returns>An async Task</returns>
        public async Task CanvasLogout(string AccessToken)
        {
            string apiEndpoint = domain + "/api/auth/canvasLogout";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("access_token", AccessToken);

            var response = await _client.DeleteAsync(apiEndpoint);
            response.EnsureSuccessStatusCode();
        }
    }
}
