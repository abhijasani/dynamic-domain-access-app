using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class OriginsController : ControllerBase
{
    private readonly OriginService _originService;

    public OriginsController(OriginService originService)
    {
        _originService = originService;
    }

    [HttpGet]
    public async Task<ActionResult<List<AllowedOrigin>>> GetAllOrigins()
    {
        var origins = await _originService.GetAllOriginsAsync();
        return Ok(origins);
    }

    [HttpPost]
    public async Task<ActionResult> AddOrigin([FromBody] string origin)
    {
        await _originService.AddOriginAsync(origin);
        return Ok(new { message = "Origin added successfully" });
    }

    [HttpDelete("{origin}")]
    public async Task<ActionResult> RemoveOrigin(string origin)
    {
        await _originService.RemoveOriginAsync(origin);
        return Ok(new { message = "Origin removed successfully" });
    }
}