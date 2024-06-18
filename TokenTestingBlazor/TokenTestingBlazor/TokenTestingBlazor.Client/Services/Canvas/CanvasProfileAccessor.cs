using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    public class CanvasProfileAccessor
    {
        private HttpClient _client;
        private readonly string domain;

        public CanvasProfileAccessor(IConfiguration Config)
        {
            _client = new HttpClient();
            domain = Config["Domain"] ?? throw new ArgumentNullException($"{nameof(domain)}");
        }

        public async Task<CanvasProfileDTO> GetCanvasProfileAsync(string token)
        {
            var apiEndpoint = domain + "/api/profile/getProfile";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<CanvasProfileDTO>(response.Content.ReadAsStream());
        }
    }
}
