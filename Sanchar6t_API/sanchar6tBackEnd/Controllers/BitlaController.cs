using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;


namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BitlaController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        private const string BITLA_BASE_URL = "https://gds-stg.ticketsimply.co.in/";
        private const string API_KEY = "TSSACPAPI73263707";

        public BitlaController(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        [HttpGet("GetCities")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCities()
        {
            var url = $"{BITLA_BASE_URL}gds/api/cities.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content);
                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch cities from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetCityPairs")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCityPairs()
        {
            var url = $"{BITLA_BASE_URL}gds/api/city_pairs.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content);
                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch city pairs from Bitla API", error = ex.Message });
            }
        }


        [HttpGet("GetMasters")]
        [AllowAnonymous]
        public async Task<IActionResult> GetMasters()
        {
            var url = $"{BITLA_BASE_URL}gds/api/masters.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content);
                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch masters from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetOperators")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperators()
        {
            var url = $"{BITLA_BASE_URL}gds/api/operators.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to fetch operators from Bitla API",
                    error = ex.Message
                });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new
                {
                    message = "Invalid JSON returned from Bitla API",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while fetching operators",
                    error = ex.Message
                });
            }
        }


        [HttpGet("GetSchedules/{originId:int}/{destinationId:int}/{travelDate}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSchedules(int originId, int destinationId, string travelDate)
        {
            var url = $"{BITLA_BASE_URL}gds/api/schedules/{originId}/{destinationId}/{travelDate}.json?api_key={API_KEY}";

            try
            {

                _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                Stream contentStream = responseStream;

                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    contentStream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }
                else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                {
                    contentStream = new System.IO.Compression.DeflateStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }


                using var reader = new StreamReader(contentStream);
                var content = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch schedule from Bitla API", error = ex.Message });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { message = "Invalid JSON returned from Bitla API", error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error occurred", error = ex.Message });
            }

        }

        [HttpGet("GetOperatorSchedules/{travelId}/{travelDate}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetOperatorSchedules(string travelId, string travelDate)
        {
            try
            {

                if (string.IsNullOrWhiteSpace(travelId) || string.IsNullOrWhiteSpace(travelDate))
                    return BadRequest(new { message = "Travel ID and Travel Date are required." });


                if (DateTime.TryParse(travelDate, out DateTime parsedDate))
                {
                    travelDate = parsedDate.ToString("yyyy-MM-dd");
                }

                var url = $"{BITLA_BASE_URL}gds/api/operator_schedules/{travelId}/{travelDate}.json?api_key={API_KEY}";

                _httpClient.DefaultRequestHeaders.Clear();
                _httpClient.DefaultRequestHeaders.Accept.Add(
                    new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(
                    new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(
                    new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                var handler = new HttpClientHandler
                {
                    AutomaticDecompression = System.Net.DecompressionMethods.GZip | System.Net.DecompressionMethods.Deflate
                };
                using var client = new HttpClient(handler);

                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new
                {
                    message = "Failed to fetch operator schedules from Bitla API",
                    error = ex.Message
                });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new
                {
                    message = "Invalid JSON returned from Bitla API",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while fetching operator schedules",
                    error = ex.Message
                });
            }
        }


        [HttpGet("GetSchedule/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetSchedule(string id)
        {
            var url = $"{BITLA_BASE_URL}gds/api/schedule/{id}.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch schedule from Bitla API", error = ex.Message });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { message = "Invalid JSON returned from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetAvailabilities/{originId:int}/{destinationId:int}/{travelDate}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailability(int originId, int destinationId, string travelDate)
        {
            var url = $"{BITLA_BASE_URL}gds/api/availabilities/{originId}/{destinationId}/{travelDate}.json?api_key={API_KEY}";

            try
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                Stream contentStream = responseStream;

                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    contentStream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }
                else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                {
                    contentStream = new System.IO.Compression.DeflateStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }

                using var reader = new StreamReader(contentStream);
                var content = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch availability from Bitla API", error = ex.Message });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { message = "Invalid JSON returned from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetAvailabilityBySchedule/{scheduleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAvailabilityBySchedule(long scheduleId)
        {
            var url = $"{BITLA_BASE_URL}gds/api/availability/{scheduleId}.json?api_key={API_KEY}";

            try
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                Stream contentStream = responseStream;

                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    contentStream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }
                else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                {
                    contentStream = new System.IO.Compression.DeflateStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }

                using var reader = new StreamReader(contentStream);
                var content = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch availability by schedule from Bitla API", error = ex.Message });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { message = "Invalid JSON returned from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetStages")]
        [AllowAnonymous]
        public async Task<IActionResult> GetStages()
        {
            var url = $"{BITLA_BASE_URL}gds/api/stages.json?api_key={API_KEY}";

            try
            {
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Clear();
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("gzip"));
                _httpClient.DefaultRequestHeaders.AcceptEncoding.Add(new System.Net.Http.Headers.StringWithQualityHeaderValue("deflate"));

                var response = await _httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                using var responseStream = await response.Content.ReadAsStreamAsync();
                Stream contentStream = responseStream;

                if (response.Content.Headers.ContentEncoding.Contains("gzip"))
                {
                    contentStream = new System.IO.Compression.GZipStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }
                else if (response.Content.Headers.ContentEncoding.Contains("deflate"))
                {
                    contentStream = new System.IO.Compression.DeflateStream(responseStream, System.IO.Compression.CompressionMode.Decompress);
                }

                using var reader = new StreamReader(contentStream);
                var content = await reader.ReadToEndAsync();

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(500, new { message = "Failed to fetch stages from Bitla API", error = ex.Message });
            }
            catch (JsonException ex)
            {
                return StatusCode(500, new { message = "Invalid JSON returned from Bitla API", error = ex.Message });
            }
        }

        [HttpGet("GetTentativeBooking/{scheduleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetTentativeBooking([FromRoute] long scheduleId)
        {
            if (scheduleId <= 0)
                return BadRequest(new { message = "Invalid Schedule ID." });

            var url = $"{BITLA_BASE_URL}gds/api/tentative_booking/{scheduleId}.json?api_key={API_KEY}";

            try
            {
                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        message = "Bitla GET tentative booking failed.",
                        statusCode = (int)response.StatusCode,
                        bitlaResponse = content
                    });
                }

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(new
                {
                    message = "Tentative booking fetched successfully.",
                    scheduleId,
                    bitlaResponse = data
                });
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(502, new { message = "Failed to communicate with Bitla API.", error = httpEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error while fetching tentative booking.", error = ex.Message });
            }
        }


        [HttpPost("PostTentativeBooking/{scheduleId}")]
        [AllowAnonymous]
        public async Task<IActionResult> PostTentativeBooking( [FromRoute] long scheduleId, [FromBody] TentativeBookingRequest? request)
        { 
            if (scheduleId <= 0)
                return BadRequest(new { message = "Schedule ID must be a valid positive number." });

            if (request == null)
                return BadRequest(new { message = "Request body cannot be null." });

            var url = $"{BITLA_BASE_URL}gds/api/tentative_booking/{scheduleId}.json?api_key={API_KEY}";

            try
            {

                var json = JsonSerializer.Serialize(request, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                using var jsonContent = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, jsonContent);
                var content = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        message = "Bitla API returned an error.",
                        statusCode = (int)response.StatusCode,
                        bitlaResponse = content
                    });
                }

                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(new
                {
                    message = "Tentative booking fetched successfully.",
                    scheduleId,
                    bitlaResponse = data
                });
            }
            catch (HttpRequestException httpEx)
            {
                return StatusCode(502, new { message = "Failed to communicate with Bitla API.", error = httpEx.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Unexpected error occurred while fetching tentative booking.", error = ex.Message });
            }
        }

        [HttpPost("PostConfirmBooking")]
        [AllowAnonymous]
        public async Task<IActionResult> PostConfirmBooking([FromQuery] string ticketNumber, [FromBody] JsonElement? confirmData)
        {
            if (string.IsNullOrEmpty(ticketNumber))
                return BadRequest(new { message = "ticketNumber is required" });

            var url = $"{BITLA_BASE_URL}gds/api/confirm_booking/{ticketNumber}.json?api_key={API_KEY}";

            try
            {
                var jsonBody = confirmData?.GetRawText() ?? "{}";

                var request = new StringContent(jsonBody, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(url, request);
                var body = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        message = "Bitla confirm failed",
                        status = response.StatusCode,
                        bitlaResponse = body
                    });
                }

                return Ok(new
                {
                    message = "Booking Confirmed Successfully",
                    ticketNumber,
                    bitlaResponse = JsonSerializer.Deserialize<object>(body)
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error",
                    error = ex.Message
                });
            }
        }



        [HttpGet("GetBookingDetails")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBookingDetails(
            [FromQuery] string? pnrNumber,
            [FromQuery] string? agentRefNumber)
        {

            if (string.IsNullOrWhiteSpace(pnrNumber) && string.IsNullOrWhiteSpace(agentRefNumber))
            {
                return BadRequest(new { message = "Either PNR Number or Agent Reference Number must be provided." });
            }

            try
            {

                var queryParams = new List<string> { $"api_key={API_KEY}" };

                if (!string.IsNullOrWhiteSpace(pnrNumber))
                    queryParams.Add($"pnr_number={pnrNumber}");

                if (!string.IsNullOrWhiteSpace(agentRefNumber))
                    queryParams.Add($"agent_ref_number={agentRefNumber}");

                var url = $"{BITLA_BASE_URL}gds/api/booking_details.json?{string.Join("&", queryParams)}";

                var response = await _httpClient.GetAsync(url);
                var content = await response.Content.ReadAsStringAsync();

                var jsonOptions = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<object>(content, jsonOptions);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        message = "Failed to fetch booking details from Bitla API",
                        bitlaResponse = data
                    });
                }

                if (content.Contains("\"code\":\"411\"") || content.Contains("\"message\":\"Ticket Not Confirmed\""))
                {
                    return Ok(new
                    {
                        message = "Ticket Not Confirmed",
                        status = 411,
                        bitlaResponse = data
                    });
                }

                return Ok(new
                {
                    message = "Booking details fetched successfully.",
                    status = 200,
                    bitlaResponse = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while fetching booking details",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("GetCanCancelTicket")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCanCancelTicket(
     [FromQuery] string ticketNumber,
     [FromQuery] string? seatNumbers = null)
        {
            if (string.IsNullOrEmpty(ticketNumber))
            {
                return BadRequest(new { message = "Ticket number is required." });
            }

            try
            {
                var url = $"{BITLA_BASE_URL}gds/api/can_cancel.json?api_key={API_KEY}&ticket_number={ticketNumber}";

                if (!string.IsNullOrEmpty(seatNumbers))
                {
                    url += $"&seat_numbers={seatNumbers}";
                }

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new
                    {
                        message = "Failed to fetch cancellable ticket info from Bitla API",
                        statusCode = (int)response.StatusCode
                    });
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(new
                {
                    message = "Cancellable ticket info fetched successfully.",
                    bitlaResponse = data
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while checking if the ticket can be cancelled.",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("getCancelBooking")]
        [AllowAnonymous]
        public async Task<IActionResult> getCancelBooking([FromQuery] string ticketNumber, [FromQuery] string seatNumbers)
        {
            if (string.IsNullOrEmpty(ticketNumber))
            {
                return BadRequest(new { message = "Ticket number is required" });
            }

            if (string.IsNullOrEmpty(seatNumbers))
            {
                return BadRequest(new { message = "Seat numbers are required" });
            }

            try
            {
                var url = $"{BITLA_BASE_URL}gds/api/cancel_booking.json?api_key={API_KEY}&ticket_number={ticketNumber}&seat_numbers={seatNumbers}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to cancel ticket from Bitla API" });
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while canceling ticket",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpGet("GetBalance")]
        [AllowAnonymous]
        public async Task<IActionResult> GetBalance([FromQuery] string travelId)
        {
            if (string.IsNullOrEmpty(travelId))
            {
                return BadRequest(new { message = "Travel ID is required" });
            }

            try
            {
                var url = $"{BITLA_BASE_URL}gds/api/get_balance.json?api_key={API_KEY}&travel_id={travelId}";

                var response = await _httpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                {
                    return StatusCode((int)response.StatusCode, new { message = "Failed to fetch balance from Bitla API" });
                }

                var content = await response.Content.ReadAsStringAsync();
                var data = JsonSerializer.Deserialize<object>(content, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });

                return Ok(data);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    message = "Unexpected error while fetching balance",
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        public class BookingDetailsRequest
        {
            public string TicketNumber { get; set; }
            public string AgentRefNumber { get; set; }
        }


        public class TentativeBookingRequest
        {
            public BookTicket? book_ticket { get; set; }
            public string? origin_id { get; set; }
            public string? destination_id { get; set; }
            public string? boarding_at { get; set; }
            public string? drop_of { get; set; }
            public string? no_of_seats { get; set; }
            public string? travel_date { get; set; }
            public CustomerGst? customer_company_gst { get; set; }
        }

        public class BookTicket
        {
            public SeatDetails? seat_details { get; set; }
            public ContactDetail? contact_detail { get; set; }
        }

        public class SeatDetails
        {
            public List<SeatDetail>? seat_detail { get; set; }
        }

        public class SeatDetail
        {
            public string? seat_number { get; set; }
            public string? fare { get; set; }
            public string? title { get; set; }
            public string? name { get; set; }
            public string? age { get; set; }
            public string? sex { get; set; }
            public string? is_primary { get; set; }
            public string? id_card_type { get; set; }
            public string? id_card_number { get; set; }
            public string? id_card_issued_by { get; set; }
        }

        public class ContactDetail
        {
            public string? mobile_number { get; set; }
            public string? emergency_name { get; set; }
            public string? email { get; set; }
        }

        public class CustomerGst
        {
            public string? name { get; set; }
            public string? gst_id { get; set; }
            public string? address { get; set; }
        }


        public class ConfirmBookingRequest
        {
            public string TicketNumber { get; set; } = string.Empty;
        }


    }

}
