using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Controllers
{
    /// <summary>
    /// Controller class used for fetching Canvas course data.
    /// </summary>
    [Route("api/courses")]
    [ApiController]
    public class CanvasCourseController : ControllerBase
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly Uri _endpoint = new Uri("https://davistech.instructure.com/api/v1/courses");

        /// <summary>
        /// Fetches all courses for the current user. [GET] /api/courses
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <returns>List of Canvas courses</returns>
        [HttpGet]
        public async Task<ActionResult<List<CanvasCourseDTO>>> GetAllCoursesAsync([FromHeader] string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await _client.GetAsync(_endpoint);
            response.EnsureSuccessStatusCode();
            return Ok(JsonSerializer.Deserialize<List<CanvasCourseDTO>>(response.Content.ReadAsStream()));
        }

        /// <summary>
        /// Fetches all students for a course. [GET] /api/courses/{id}/students
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <param name="id">Course id</param>
        /// <returns>List of Canvas Students</returns>
        [HttpGet("{id}/students")]
        public async Task<ActionResult<List<CanvasStudentDTO>>> GetStudentsInCourse([FromHeader] string token, int id)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await _client.GetAsync(_endpoint + $"/{id}/students");
            response.EnsureSuccessStatusCode();
            return Ok(JsonSerializer.Deserialize<List<CanvasStudentDTO>>(response.Content.ReadAsStream()));
        }
    }
}
