using Microsoft.AspNetCore.Mvc;
using System.Text.Json;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Controllers
{
    /// <summary>
    /// Controller class for fetching Canvas profile data.
    /// </summary>
    [Route("api/profile")]
    [ApiController]
    public class CanvasProfileController : ControllerBase
    {

        private readonly HttpClient _client = new HttpClient();
        private readonly Uri _endpoint = new Uri("https://davistech.instructure.com/api/v1/users/self/profile");

        /// <summary>
        /// Get the user's profile. Requires a valid access token. [GET] /api/profile
        /// </summary>
        /// <param name="token">Canvas access token</param>
        /// <returns>Canvas user profile</returns>
        public async Task<ActionResult<CanvasProfile>> GetCanvasProfileAsync([FromHeader] string token)
        {
            _client.DefaultRequestHeaders.Add("Authorization", "Bearer " + token);
            var response = await _client.GetAsync(_endpoint);
            response.EnsureSuccessStatusCode();
            return Ok(JsonSerializer.Deserialize<CanvasProfile>(response.Content.ReadAsStream()));
        }

    }
}
