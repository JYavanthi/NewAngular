using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Microsoft.AspNetCore.Authorization;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhonePeController : ControllerBase
    {
        private readonly ILogger<PhonePeController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _merchantId = "TEST-M222NJL8ZHVEM_25041";
        private readonly string _saltKey = "NjIxZTdiZGYtMzlkOS00ZTkyLWFhNjItZTZhNTBjNTgyM2I0";
        private readonly string _saltIndex = "1";
        private readonly bool _isProd = false;


        private readonly string _uatBaseUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox";
        private readonly string _prodBaseUrl = "https://api.phonepe.com/apis/pg";

        public PhonePeController(ILogger<PhonePeController> logger, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        [HttpPost("CreateOrder")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateOrder([FromBody] OrderRequestV2 request)
        {
            try
            {
                _logger.LogInformation("Creating order with PhonePe Standard Checkout");


                if (request == null)
                {
                    return BadRequest("Request cannot be null");
                }

                if (request.MerchantOrderId == null)
                {
                    return BadRequest("MerchantOrderId is required");
                }

                if (request.Amount <= 0)
                {
                    return BadRequest("Amount must be greater than 0");
                }


                var tokenResult = await GetOAuthTokenWithDetails();
                if (!tokenResult.Success)
                {
                    return BadRequest(new { error = "Failed to get OAuth token", details = tokenResult.ErrorDetails });
                }

                string accessToken = tokenResult.Token;
                _logger.LogInformation($"Obtained OAuth token: {accessToken}");


                if (request.PaymentFlow == null)
                {
                    request.PaymentFlow = new PaymentFlowV2 { Type = "PG_CHECKOUT" };
                }


                if (request.PaymentFlow.MerchantUrls == null)
                {
                    request.PaymentFlow.MerchantUrls = new MerchantUrls
                    {
                        RedirectUrl = "https://your-redirect-url.com/callback"
                    };
                }


                string apiPath = "/checkout/v2/pay";


                string baseUrl = _isProd ? _prodBaseUrl : _uatBaseUrl;
                string apiUrl = $"{baseUrl}{apiPath}";

                _logger.LogInformation($"API URL: {apiUrl}");


                var httpClient = _httpClientFactory.CreateClient();


                var requestMessage = new HttpRequestMessage(HttpMethod.Post, apiUrl);


                requestMessage.Headers.Add("Authorization", $"O-Bearer {accessToken}");


                string jsonPayload = JsonConvert.SerializeObject(request);
                _logger.LogInformation($"Request Body: {jsonPayload}");


                var content = new StringContent(
                    jsonPayload,
                    Encoding.UTF8,
                    "application/json"
                );


                requestMessage.Content = content;


                _logger.LogInformation("Sending request to PhonePe API");
                var response = await httpClient.SendAsync(requestMessage);
                var responseContent = await response.Content.ReadAsStringAsync();

                _logger.LogInformation($"Response Status: {response.StatusCode}");
                _logger.LogInformation($"Response Content: {responseContent}");

                if (response.IsSuccessStatusCode)
                {

                    var phonepeResponse = JsonConvert.DeserializeObject<StandardCheckoutResponse>(responseContent);
                    return Ok(phonepeResponse);
                }
                else
                {
                    _logger.LogError($"PhonePe API error - Status: {response.StatusCode}, Content: {responseContent}");


                    try
                    {
                        var errorObj = JsonConvert.DeserializeObject<ErrorResponse>(responseContent);
                        return StatusCode((int)response.StatusCode, new { code = errorObj?.Code, message = errorObj?.Message });
                    }
                    catch
                    {
                        return StatusCode((int)response.StatusCode, responseContent);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating PhonePe order");
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }

        private class TokenResult
        {
            public bool Success { get; set; }
            public string Token { get; set; }
            public string ErrorDetails { get; set; }
        }

        private async Task<TokenResult> GetOAuthTokenWithDetails()
        {
            try
            {

                string clientId = _merchantId;
                string clientSecret = _saltKey;
                string clientVersion = "1";


                string baseUrl = _isProd ? _prodBaseUrl : _uatBaseUrl;


                string tokenUrl = $"{baseUrl}/v1/oauth/token";
                _logger.LogInformation($"OAuth Token URL: {tokenUrl}");


                var httpClient = _httpClientFactory.CreateClient();
                httpClient.Timeout = TimeSpan.FromSeconds(30);


                var formData = new Dictionary<string, string>
                {
                    { "client_id", clientId },
                    { "client_secret", clientSecret },
                    { "client_version", clientVersion },
                    { "grant_type", "client_credentials" }
                };

                _logger.LogInformation($"OAuth Request - client_id: {clientId}");
                _logger.LogInformation($"OAuth Request - client_version: {clientVersion}");
                _logger.LogInformation($"OAuth Request - grant_type: client_credentials");


                var content = new FormUrlEncodedContent(formData);


                _logger.LogInformation("Sending OAuth token request to: " + tokenUrl);

                try
                {
                    var response = await httpClient.PostAsync(tokenUrl, content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    _logger.LogInformation($"OAuth Response Status: {response.StatusCode}");
                    _logger.LogInformation($"OAuth Response Content: {responseContent}");

                    if (response.IsSuccessStatusCode)
                    {
                        try
                        {
                            var tokenResponse = JsonConvert.DeserializeObject<TokenResponse>(responseContent);
                            if (tokenResponse?.AccessToken != null)
                            {
                                return new TokenResult { Success = true, Token = tokenResponse.AccessToken };
                            }
                            else
                            {
                                return new TokenResult
                                {
                                    Success = false,
                                    ErrorDetails = "Token response was successful but contained no access_token"
                                };
                            }
                        }
                        catch (Exception ex)
                        {
                            return new TokenResult
                            {
                                Success = false,
                                ErrorDetails = $"Failed to parse token response: {ex.Message}. Response: {responseContent}"
                            };
                        }
                    }
                    else
                    {
                        return new TokenResult
                        {
                            Success = false,
                            ErrorDetails = $"OAuth request failed with status {response.StatusCode}: {responseContent}"
                        };
                    }
                }
                catch (HttpRequestException ex)
                {
                    _logger.LogError(ex, "HTTP request exception during OAuth token request");
                    return new TokenResult
                    {
                        Success = false,
                        ErrorDetails = $"HTTP request failed: {ex.Message}"
                    };
                }
                catch (TaskCanceledException ex)
                {
                    _logger.LogError(ex, "Request timeout during OAuth token request");
                    return new TokenResult
                    {
                        Success = false,
                        ErrorDetails = "Request timed out. The PhonePe server might be slow or unreachable."
                    };
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting OAuth token");
                return new TokenResult
                {
                    Success = false,
                    ErrorDetails = $"Exception: {ex.Message}"
                };
            }
        }


        private async Task<string> GetOAuthToken()
        {
            var result = await GetOAuthTokenWithDetails();
            return result.Success ? result.Token : null;
        }

        [HttpGet("test-token")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTestOAuthToken()
        {
            try
            {
                _logger.LogInformation("Requesting OAuth token directly from PhonePe with test credentials");


                string clientId = _merchantId;
                string clientSecret = _saltKey;
                string clientVersion = "1";

                using (var httpClient = new HttpClient())
                {
                    httpClient.Timeout = TimeSpan.FromSeconds(30);


                    string tokenUrl = "https://api-preprod.phonepe.com/apis/pg-sandbox/v1/oauth/token";


                    var formData = new Dictionary<string, string>
                    {
                        { "client_id", clientId },
                        { "client_secret", clientSecret },
                        { "client_version", clientVersion },
                        { "grant_type", "client_credentials" }
                    };

                    _logger.LogInformation($"OAuth Request URL: {tokenUrl}");
                    _logger.LogInformation($"OAuth Request Params: client_id={clientId}, client_version={clientVersion}, grant_type=client_credentials");


                    var content = new FormUrlEncodedContent(formData);


                    try
                    {
                        var response = await httpClient.PostAsync(tokenUrl, content);
                        var responseContent = await response.Content.ReadAsStringAsync();

                        _logger.LogInformation($"OAuth Test Response Status: {response.StatusCode}");
                        _logger.LogInformation($"OAuth Test Response Content: {responseContent}");


                        return new ContentResult
                        {
                            Content = JsonConvert.SerializeObject(new
                            {
                                statusCode = (int)response.StatusCode,
                                isSuccessful = response.IsSuccessStatusCode,
                                content = responseContent
                            }),
                            ContentType = "application/json",
                            StatusCode = 200
                        };
                    }
                    catch (Exception reqEx)
                    {
                        _logger.LogError(reqEx, "HTTP request exception during test OAuth token request");
                        return StatusCode(500, new
                        {
                            error = "Request failed",
                            message = reqEx.Message,
                            innerException = reqEx.InnerException?.Message
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in test token endpoint");
                return StatusCode(500, new
                {
                    Error = ex.Message,
                    StackTrace = ex.StackTrace
                });
            }
        }
    }


    public class OrderRequestV2
    {
        [JsonProperty("merchantOrderId")]
        public string MerchantOrderId { get; set; }

        [JsonProperty("amount")]
        public int Amount { get; set; }

        [JsonProperty("expireAfter")]
        public int ExpireAfter { get; set; } = 1200;

        [JsonProperty("metaInfo")]
        public MetaInfo MetaInfo { get; set; } = new MetaInfo();

        [JsonProperty("paymentFlow")]
        public PaymentFlowV2 PaymentFlow { get; set; } = new PaymentFlowV2();
    }

    public class PaymentFlowV2
    {
        [JsonProperty("type")]
        public string Type { get; set; } = "PG_CHECKOUT";

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("merchantUrls")]
        public MerchantUrls MerchantUrls { get; set; } = new MerchantUrls();

        [JsonProperty("paymentModeConfig")]
        public PaymentModeConfig PaymentModeConfig { get; set; } = new PaymentModeConfig();
    }

    public class MerchantUrls
    {
        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }
    }

    public class PaymentModeConfig
    {
        [JsonProperty("enabledPaymentModes")]
        public List<PaymentMode> EnabledPaymentModes { get; set; } = new List<PaymentMode>();

        [JsonProperty("disabledPaymentModes")]
        public List<PaymentMode> DisabledPaymentModes { get; set; } = new List<PaymentMode>();
    }

    public class PaymentMode
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("cardTypes")]
        public List<string> CardTypes { get; set; } = new List<string>();
    }

    public class MetaInfo
    {
        [JsonProperty("udf1")]
        public string Udf1 { get; set; }

        [JsonProperty("udf2")]
        public string Udf2 { get; set; }

        [JsonProperty("udf3")]
        public string Udf3 { get; set; }

        [JsonProperty("udf4")]
        public string Udf4 { get; set; }

        [JsonProperty("udf5")]
        public string Udf5 { get; set; }
    }

    public class StandardCheckoutResponse
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("state")]
        public string State { get; set; }

        [JsonProperty("expireAt")]
        public long? ExpireAt { get; set; }

        [JsonProperty("redirectUrl")]
        public string RedirectUrl { get; set; }
    }

    public class ErrorResponse
    {
        [JsonProperty("code")]
        public string Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }

    public class TokenResponse
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("token_type")]
        public string TokenType { get; set; }

        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
    }
}