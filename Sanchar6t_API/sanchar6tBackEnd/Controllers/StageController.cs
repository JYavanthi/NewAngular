//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Mvc;

//namespace sanchar6tBackEnd.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class StageController : ControllerBase
//    {
//        private readonly IHttpClientFactory _httpClientFactory;
//        private const string BITLA_BASE_URL = "http://gds-stg.ticketsimply.co.in/";
//        private const string API_KEY = "TSSACPAPI73263707";

//        public StageController(IHttpClientFactory httpClientFactory)
//        {
//            _httpClientFactory = httpClientFactory;
//        }
//        [HttpGet("BitlaStages")]
//        public async Task<IActionResult> GetStages()
//        {
//            try
//            {
//                var client = _httpClientFactory.CreateClient();
//                var response = await client.GetAsync($"{BITLA_BASE_URL}gds/api/stages.json?api_key={API_KEY}");

//                if (!response.IsSuccessStatusCode)
//                    return StatusCode((int)response.StatusCode, "Failed to fetch data from Bitla API");

//                var content = await response.Content.ReadAsStringAsync();
//                return Content(content, "application/json");
//            }
//            catch (HttpRequestException ex)
//            {
//                return StatusCode(500, $"Error connecting to Bitla API: {ex.Message}");
//            }
//        }
//    }
//}
