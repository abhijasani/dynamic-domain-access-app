using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly JwtService _jwtService;

    public AuthController(JwtService jwtService) => _jwtService = jwtService;

    [HttpPost("login")]
    [AllowAnonymous]
    public IActionResult Login([FromBody] LoginModel model)
    {
        var token = _jwtService.GenerateToken(model.Username);
        return Ok(new { token });
    }
}

public class LoginModel
{
    public string Username { get; set; }
    public string Password { get; set; }
}