using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TestController : ControllerBase
{
    [HttpGet("secure")]
    public IActionResult GetData()
    {
        return Ok(new { message = "You are authorized" });
    }
}