/*using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace sanchar6tBackEnd.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OTPController : ControllerBase
    {
    }
}*/
using Microsoft.AspNetCore.Mvc;

[Route("api/otp")]
[ApiController]
public class OTPController : ControllerBase
{
    /*private readonly OTPService _otpService;
    public OTPController(OTPService otpService) { _otpService = otpService; }
*/
    /*[HttpPost("generate")]
    public IActionResult GenerateOTP([FromBody] string user)
    {
        string otp = _otpService.GenerateOTP(user);
        return Ok(new { message = "OTP generated!", otp });
    }

    [HttpPost("validate")]
    public IActionResult ValidateOTP([FromBody] OTPRequest request)
    {
        return _otpService.ValidateOTP(request.User, request.OTP)
            ? Ok(new { message = "OTP valid!" })
            : BadRequest(new { message = "Invalid OTP!" });
    }*/
}

public class OTPRequest
{
    public string User { get; set; }
    public string OTP { get; set; }
}

