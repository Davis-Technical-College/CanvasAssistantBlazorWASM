using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using System.Net.Http.Headers;
using TokenTestingBlazor.Models;

namespace TokenTestingBlazor.Controllers
{
    [Route("api/profile")]
    [ApiController]
    public class CanvasProfileController : ControllerBase
    {

        private readonly HttpClient _client = new HttpClient();

        // GET: /api/profile/getProfile ??? Getting 404s...
        /// <summary>
        /// Get the user's profile. Requires a valid refresh token.
        /// </summary>
        /// <param name="token">Canvas refresh token</param>
        /// <returns>Canvas user</returns>
        [HttpGet("getProfile")]
        public async Task<ActionResult<ServerCanvasProfileDTO>> GetCanvasProfileAsync([FromHeader] string token)
        {
            Console.WriteLine("Token: " + token);

            var request = new HttpRequestMessage();
            request.RequestUri = new Uri("https://davistech.instructure.com/api/v1/users/self/profile");
            request.Method = HttpMethod.Get;
            request.Headers.Add("Authorization", "Bearer " + token);

            Console.WriteLine(request.Headers);

            var response = await _client.SendAsync(request);

            response.EnsureSuccessStatusCode();

            return Ok(JsonSerializer.Deserialize<ServerCanvasProfileDTO>(response.Content.ReadAsStream()));
        }

    }
}
