using System.Net;
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
        /// Fetches courses from the server API with pagination support.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="pageNumber">Current page number</param>
        /// <param name="itemsPerPage">Number of items per page</param>
        /// <returns>List of Canvas courses</returns>
        public async Task<List<CanvasCourse>> FetchAllCourses(string token, int pageNumber, int itemsPerPage)
        {
            Uri apiEndpoint = new Uri(domain + $"/api/courses?page={pageNumber}&perPage={itemsPerPage}");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            HttpResponseMessage response = await _client.GetAsync(apiEndpoint);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                return JsonSerializer.Deserialize<List<CanvasCourse>>(response.Content.ReadAsStream());
            }
            else
            {
                // Handle non-successful response (e.g., log error, throw exception, etc.)
                return new List<CanvasCourse>();
            }
        }


        /// <summary>
        /// Fetches all students for a course.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="id">Canvas course id</param>
        /// <returns>List of students</returns>
        public async Task<List<CanvasStudent>> FetchStudentsInCourse(string token, int id)
        {
            Uri endpoint = new Uri(domain + $"/api/courses/{id}/students");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            HttpResponseMessage response = await _client.GetAsync(endpoint);

            return JsonSerializer.Deserialize<List<CanvasStudent>>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Fetches all assignments for a course.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="id">Canvas course id</param>
        /// <returns>List of Canvas assignments</returns>
        public async Task<List<CanvasAssignment>> FetchAssignmentsForCourse(string token, int id)
        {
            Uri endpoint = new Uri(domain + $"/api/courses/{id}/assignments");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            HttpResponseMessage response = await _client.GetAsync(endpoint);

            return JsonSerializer.Deserialize<List<CanvasAssignment>>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Fetches all modules for a course.
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="id">Canvas course id</param>
        /// <returns>List of Canvas modules</returns>
        public async Task<List<CanvasCourseModule>> FetchModulesForCourse(string token, int id)
        {
            Uri endpoint = new Uri(domain + $"/api/courses/{id}/modules");

            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("token", token);

            HttpResponseMessage response = await _client.GetAsync(endpoint);

            return JsonSerializer.Deserialize<List<CanvasCourseModule>>(response.Content.ReadAsStream());
        }
    }
}
