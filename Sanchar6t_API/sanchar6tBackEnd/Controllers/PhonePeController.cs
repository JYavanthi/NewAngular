using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authorization;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonePeTestController : ControllerBase
    {
        private readonly ILogger<PhonePeTestController> _logger;

        public PhonePeTestController(ILogger<PhonePeTestController> logger)
        {
            _logger = logger;
        }

        [HttpGet("test-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestOAuthToken()
        {
            try
            {
                _logger.LogInformation("Requesting OAuth token from PhonePe with test credentials");


                string clientId = "SU2512041519267109485044";
                string clientSecret = "e1babbea-ec50-4ac6-9b46-9a2ce64a5e04";
                string clientVersion = "1";

                using (var httpClient = new HttpClient())
                {

                    string tokenUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";

                    var formData = new Dictionary<string, string>
                    {
                        { "client_id", clientId },
                        { "client_version", clientVersion },
                        { "client_secret", clientSecret },
                        { "grant_type", "client_credentials" }
                    };

                    var content = new FormUrlEncodedContent(formData);

                    var response = await httpClient.PostAsync(tokenUrl, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    return new ContentResult
                    {
                        Content = responseContent,
                        ContentType = "application/json",
                        StatusCode = (int)response.StatusCode
                    };
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
    }
}