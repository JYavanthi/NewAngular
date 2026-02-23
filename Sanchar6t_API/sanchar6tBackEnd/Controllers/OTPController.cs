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

}

public class OTPRequest
{
    public string User { get; set; }
    public string OTP { get; set; }
}

