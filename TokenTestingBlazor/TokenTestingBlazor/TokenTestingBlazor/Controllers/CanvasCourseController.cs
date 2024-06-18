using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TokenTestingBlazor.Models;

namespace TokenTestingBlazor.Controllers
{
    [Route("api/courses")]
    [ApiController]
    public class CanvasCourseController : ControllerBase
    {
        private readonly HttpClient _client = new HttpClient();
        private readonly Uri _endpoint = new Uri("https://davistech.instructure.com/api/v1/courses");

        [HttpGet]
        public async Task<ActionResult<List<ServerCanvasCourseDTO>>> GetAllCoursesAsync([FromHeader] string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            HttpResponseMessage response = await _client.GetAsync(_endpoint);

            response.EnsureSuccessStatusCode();

            Console.WriteLine(await response.Content.ReadAsStringAsync());

            return Ok(JsonSerializer.Deserialize<List<ServerCanvasCourseDTO>>(response.Content.ReadAsStream()));
        }
    }
}
