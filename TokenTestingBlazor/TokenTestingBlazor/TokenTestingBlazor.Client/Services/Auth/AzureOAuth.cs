using Microsoft.AspNetCore.Components;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Web;
using TokenTestingBlazor.Client.Models;

namespace TokenTestingBlazor.Client
{
    /// <summary>
    /// Utility Service for getting the Microsoft Access Token
    /// <see href="https://learn.microsoft.com/en-us/entra/identity-platform/v2-oauth2-auth-code-flow"/>
    /// </summary>
    public class AzureOAuth
    {
#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        /// <summary>
        /// The Directory (tenant) ID of the registered Entra ID application
        /// </summary>
        private static string tenant;

        /// <summary>
        /// The Application (client) ID of the registered Entra ID application
        /// </summary>
        private static string client_id;

        /// <summary>
        /// The URI redirected to after authentication
        /// </summary>
        private static string redirect_uri;

        /// <summary>
        /// The URI of the CosmosDB Instance
        /// </summary>
        private static string cosmosURI;
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

        private HttpClient client;

        public AzureOAuth(IConfiguration Config)
        {
            client = new HttpClient();
            tenant = Config["Azure:tenant"] ?? throw new ArgumentNullException();
            client_id = Config["Azure:client_id"] ?? throw new ArgumentNullException();
            redirect_uri = Config["Azure:redirect_uri"] ?? throw new ArgumentNullException();
            cosmosURI = Config["Azure:cosmos_uri"] ?? throw new ArgumentNullException();
        }


        /// <summary>
        /// Gets the auth code using the Microsoft Identity Platform using the client/tenant id configured in AzureOAuth.cs and redirects to the redirect_uri configured in the same place
        /// </summary>
        /// <param name="navigationManager">Navigation Manager for a Blazor WASM</param>
        public void GetAuthCode(NavigationManager navigationManager)
        {
            var (challenge, verify) = Generate();


            var endpoint = $"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/authorize?";
            endpoint += $"client_id={client_id}";
            endpoint += $"&response_type=code";
            endpoint += $"&redirect_uri={redirect_uri}";
            endpoint += $"&state={verify}";
            endpoint += $"&scope={cosmosURI}.default"; //https://learn.microsoft.com/en-us/entra/identity-platform/scopes-oidc
            endpoint += $"&code_challenge={challenge}";
            endpoint += $"&code_challenge_method=S256";

            navigationManager.NavigateTo(endpoint);
        }


        /// <summary>
        /// Retrives the Microsoft Identity Access Token given the authorization code and the verification state
        /// </summary>
        /// <param name="authCode">Code retrieved from the GetAuthCode method</param>
        /// <param name="state">Verification state returned by the callback url</param>
        /// <returns>A Task that resolves to the access token</returns>
        public async Task<TokenDTO> GetAccessToken(string authCode, string state)
        {
            var endpoint = new Uri($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token");

            var values = new Dictionary<string, string>
            {
                { "client_id", client_id },
                { "code", authCode },
                { "redirect_uri", redirect_uri },
                { "code_verifier", state },
                { "grant_type", "authorization_code" },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TokenDTO>(response.Content.ReadAsStream());

        }

        public async Task<TokenDTO> RefreshAccessToken(string refreshToken)
        {
            var endpoint = new Uri($"https://login.microsoftonline.com/{tenant}/oauth2/v2.0/token");

            var values = new Dictionary<string, string>
            {
                {"client_id", client_id },
                { "grant_type", "refresh_token" },
                { "refresh_token", refreshToken },
                { "scope", $"{cosmosURI}.default" },
            };

            var requestContent = new FormUrlEncodedContent(values);

            var response = await client.PostAsync(endpoint.ToString(), requestContent);
            response.EnsureSuccessStatusCode();

            return JsonSerializer.Deserialize<TokenDTO>(response.Content.ReadAsStream());
        }

        /// <summary>
        /// Creates the PKCE key used for authorization
        /// </summary>
        /// <param name="size">Length of the key</param>
        /// <returns>The hashed key and the value used to verify the key</returns>
        private static (string code_challenge, string verifier) Generate(int size = 43)
        {
            using var rng = RandomNumberGenerator.Create();
            var randomBytes = new byte[size];
            rng.GetBytes(randomBytes);
            var verifier = Base64UrlEncode(randomBytes);

            var buffer = Encoding.UTF8.GetBytes(verifier);
            var hash = SHA256.Create().ComputeHash(buffer);
            var challenge = Base64UrlEncode(hash);

            return (challenge, verifier);
        }


        private static string Base64UrlEncode(byte[] data) =>
            Convert.ToBase64String(data)
                .Replace("+", "-")
                .Replace("/", "_")
                .TrimEnd('=');
    }
}
