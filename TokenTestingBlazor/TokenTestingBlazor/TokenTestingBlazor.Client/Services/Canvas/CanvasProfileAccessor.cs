using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    /// <summary>
    /// Utility class for fetching Canvas profile data from the server API.
    /// </summary>
    public class CanvasProfileAccessor
    {
        private HttpClient _client;
        private readonly string domain;

        public CanvasProfileAccessor(IConfiguration Config)
        {
            _client = new HttpClient();
            domain = Config["Domain"] ?? throw new ArgumentNullException($"{nameof(domain)}");
        }

        /// <summary>
        /// Fetches the user profile from the server API.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <returns>Canvas user profile</returns>
        public async Task<CanvasProfile> GetCanvasProfileAsync(string token)
        {
            var apiEndpoint = domain + "/api/profile";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<CanvasProfile>(response.Content.ReadAsStream());
        }
    }
}
