using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using sanchar6tBackEnd.Helpers;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentGatewayController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private const string MerchantId = "M222NJL8ZHVEM";
        private const string SaltKey = "3013c44a-99b1-4482-88b7-b1387e079b49";
        private const string SaltIndex = "1";
        private const string BaseUrl = "https://api-preprod.phonepe.com/apis/hermes/pg/v1";
        public PaymentGatewayController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpPost("initiate")]
        [AllowAnonymous]
        public async Task<IActionResult> InitiatePaymentAsync(string orderId, decimal amount)
        {
            string callbackUrl = "http://localhost:5086/api/payment/callback";

            orderId ??= Guid.NewGuid().ToString("N");

            var requestData = new
            {
                merchantId = MerchantId,
                merchantTransactionId = orderId,
                amount = (int)(amount * 100),
                currency = "INR",
                redirectUrl = callbackUrl,
                callbackUrl = callbackUrl,
                paymentInstrument = new { type = "UPI_INTENT" }
            };

            string jsonPayload = JsonConvert.SerializeObject(requestData);
            string base64Payload = Convert.ToBase64String(Encoding.UTF8.GetBytes(jsonPayload));

            string xVerify = GenerateXVerify(base64Payload);

            using var client = _httpClient;
            client.DefaultRequestHeaders.Clear();
            client.DefaultRequestHeaders.Add("X-VERIFY", xVerify);
            client.DefaultRequestHeaders.Add("X-MERCHANT-ID", MerchantId);

            var content = new StringContent(JsonConvert.SerializeObject(new { request = base64Payload }), Encoding.UTF8, "application/json");

            var response = await client.PostAsync("https://api-preprod.phonepe.com/apis/pg-sandbox/pg/v1/pay", content);
            string responseData = await response.Content.ReadAsStringAsync();

            return Ok(new { orderId, response = responseData });

        }


        private string GenerateXVerify(string base64Payload)
        {
            string apiEndpoint = "/pg-sandbox/pg/v1/pay";
            string dataToSign = base64Payload + apiEndpoint + SaltKey;

            using var hmac = new HMACSHA256(Encoding.UTF8.GetBytes(SaltKey));
            byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(dataToSign));
            string hex = BitConverter.ToString(hash).Replace("-", "").ToLower();

            return $"{hex}###{SaltIndex}";
        }



        [HttpPost("payment/callback")]
        [AllowAnonymous]
        public async Task<IActionResult> PaymentCallback([FromBody] PhonePeCallbackModel callbackData)
        {
            Console.WriteLine($"Callback received: {JsonConvert.SerializeObject(callbackData)}");

            return Ok();
        }
    }
    public class PhonePeCallbackModel
    {
        public string transactionId { get; set; }
        public string merchantId { get; set; }
        public string merchantTransactionId { get; set; }
        public int amount { get; set; }
        public string state { get; set; }
        public string responseCode { get; set; }
        public PaymentInstrument paymentInstrument { get; set; }
    }

    public class PaymentInstrument
    {
        public PaymentInstrument()
        {
        }

        public string type { get; set; }
        public string utr { get; set; }
    }
    public class PaymentRequest
    {
        public decimal Amount { get; set; }
        public string OrderId { get; set; }
    }
}

