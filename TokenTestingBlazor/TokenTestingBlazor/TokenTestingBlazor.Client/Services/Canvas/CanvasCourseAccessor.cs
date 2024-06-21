using System.Text.Json;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    /// <summary>
    /// Utility class for fetching Canvas course data.
    /// </summary>
    public class CanvasCourseAccessor
    {
        private HttpClient _client;
        private readonly string domain;

        public CanvasCourseAccessor(IConfiguration Config)
        {
            _client = new HttpClient();
            domain = Config["Domain"] ?? throw new ArgumentNullException($"{nameof(domain)}");
        }

        /// <summary>
        /// Fetches all courses from the server API.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <returns>List of Canvas courses</returns>
        public async Task<List<CanvasCourseDTO>> FetchAllCourses(string token)
        {
            var apiEndpoint = domain + "/api/courses";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            var response = await _client.GetAsync(apiEndpoint);

            return JsonSerializer.Deserialize<List<CanvasCourseDTO>>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Fetches all students for a course.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="id">Canvas course id</param>
        /// <returns>List of students</returns>
        public async Task<List<CanvasStudentDTO>> FetchStudentsInCourse(string token, int id)
        {
            var endpoint = domain + $"/api/courses/{id}/students";

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            var response = await _client.GetAsync(endpoint);

            return JsonSerializer.Deserialize<List<CanvasStudentDTO>>(response.Content.ReadAsStream());
        }
    }
}
