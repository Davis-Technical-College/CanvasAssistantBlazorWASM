using Microsoft.AspNetCore.Mvc;
using TokenTestingBlazor.Models;

namespace TokenTestingBlazor.Controllers
{
    /// <summary>
    /// API Controller used for Canvas Authentication
    /// </summary>
    [Route("api/auth")]
    [ApiController]
    public class CanvasAuthController : ControllerBase
    {

        private readonly CanvasOAuth _canvasAuth;
        public CanvasAuthController(CanvasOAuth canvasAuth) 
        {
            _canvasAuth = canvasAuth;
        }

        //GET: api/auth/getToken
        /// <summary>
        /// API Endpoint to exchange the Canvas Auth Code for an Access Token
        /// </summary>
        /// <param name="code">Canvas Auth Code</param>
        /// <param name="AzureToken">CosmosDB Access Token</param>
        /// <returns>Canvas Access Token</returns>
        [HttpGet("getToken")]
        public async Task<ActionResult<ServerCanvasTokenDTO>> GetCanvasToken(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return NotFound();
            }

            var token = await _canvasAuth.GetCanvasTokenAsync(code);
            return Ok(token);
        }

        //GET: api/auth/refreshToken
        /// <summary>
        /// Refreshes the Canvas token
        /// </summary>
        /// <param name="refresh_token">Refresh Token in the request header</param>
        /// <param name="AzureToken">CosmosDB Access Token</param>
        /// <returns>Canvas Access Token</returns>
        [HttpGet("refreshToken")]
        public async Task<ActionResult<ServerCanvasRefreshDTO>> RefreshCanvasToken([FromHeader] string? refresh_token)
        {
            if (refresh_token == null)
            {
                return NotFound();
            }
            
            var token = await _canvasAuth.RefreshCanvasTokenAsync(refresh_token);
            return Ok(token);
        }

        //DELETE: api/auth/canvasLogout
        /// <summary>
        /// Logs a user out of canvas
        /// </summary>
        /// <param name="access_token">Canvas Access Token which will be deleted in the logout process</param>
        /// <returns></returns>
        [HttpDelete("canvasLogout")]
        public async Task<ActionResult> CanvasLogout([FromHeader] string? access_token)
        {
            if (access_token == null)
            {
                return NotFound();
            }

            bool success = await _canvasAuth.CanvasLogout(access_token);
            if (success)
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}
