using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Text.Json;
using TokenTestingBlazor.Client.Models;
using TokenTestingBlazor.Models;

namespace TokenTestingBlazor
{
    /// <summary>
    /// Utility Class for getting canvas OAuth tokens
    /// <see href="https://canvas.instructure.com/doc/api/file.oauth.html"/>
    /// </summary>
    public class CanvasOAuth
    {
        //Values from appsettings.json
        private readonly string oAuthClientID;
        private readonly string oAuthClientSecret;
        private readonly string redirectURI;
        private readonly string tokenURI;

        private readonly HttpClient client;
        public CanvasOAuth(IConfiguration Config)
        {
            client = new HttpClient();
            oAuthClientID = Config["Canvas:client_id"] ?? throw new ArgumentNullException(nameof(oAuthClientID));
            oAuthClientSecret = Config["Canvas:client_secret"] ?? throw new ArgumentNullException(nameof(oAuthClientSecret));
            redirectURI = Config["Canvas:redirect_uri"] ?? throw new ArgumentNullException(nameof(redirectURI));
            tokenURI = Config["Canvas:token_uri"] ?? throw new ArgumentNullException(nameof(tokenURI));
        }

        /// <summary>
        /// Exchanges the Auth code for the Canvas access token, getting the client secret from the CosmosDB instance
        /// </summary>
        /// <param name="authCode">Authorization Code from canvas</param>
        /// <returns>A DTO containing the Canvas Access Token</returns>
        public async Task<ServerCanvasTokenDTO> GetCanvasTokenAsync(string authCode)
        {
            var endpoint = new Uri(tokenURI);

            var values = new Dictionary<string, string>
            {
                { "grant_type", "authorization_code" },
                { "client_id", oAuthClientID },
                { "client_secret", oAuthClientSecret },
                { "redirect_uri", redirectURI },
                {"code", authCode },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();



            return JsonSerializer.Deserialize<ServerCanvasTokenDTO>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Refreshes the Canvas Access Token
        /// </summary>
        /// <param name="refreshToken"></param>
        /// <returns></returns>
        public async Task<ServerCanvasRefreshDTO> RefreshCanvasTokenAsync(string refreshToken)
        {
            var endpoint = new Uri(tokenURI);

            var values = new Dictionary<string, string>()
            {
                {"grant_type", "refresh_token" },
                {"client_id", oAuthClientID },
                { "client_secret", oAuthClientSecret },
                { "redirect_uri", redirectURI },
                { "refresh_token", refreshToken },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<ServerCanvasRefreshDTO>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Logs the user out of canvas
        /// </summary>
        /// <param name="AccessToken">User's access token</param>
        /// <returns>A boolean indicating if the user was successfully logged out</returns>
        public async Task<bool> CanvasLogout(string AccessToken)
        {
            var endpoint = tokenURI + "?access_token=" + AccessToken;

            var response = await client.DeleteAsync(endpoint);

            return response.IsSuccessStatusCode;
        }

    }
}
