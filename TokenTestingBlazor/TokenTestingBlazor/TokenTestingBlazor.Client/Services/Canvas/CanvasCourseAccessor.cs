using System.Text.Json;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    public class CanvasCourseAccessor
    {
        private HttpClient _client;
        private readonly string domain;

        public CanvasCourseAccessor(IConfiguration Config)
        {
            _client = new HttpClient();
            domain = Config["Domain"] ?? throw new ArgumentNullException($"{nameof(domain)}");
        }

        public async Task<List<CanvasCourseDTO>> FetchAllCourses(string token)
        {
            var apiEndpoint = domain + "/api/courses";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<List<CanvasCourseDTO>>(response.Content.ReadAsStream());
        }
    }
}
