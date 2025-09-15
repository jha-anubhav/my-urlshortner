using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MyUrlShortner
{
[ApiController]
[Route("api/[controller]")]
public class urlshortnerController : ControllerBase
{
    private readonly IUrlShortner _urlShortnerService;

    public urlshortnerController(IUrlShortner urlShortnerService)
    {
        _urlShortnerService = urlShortnerService;
    }

    [HttpPost("shorten")]
    public IActionResult ShortenUrl([FromBody] string originalUrl)
    {
        try
        {
            var shortenedUrl = _urlShortnerService.ShortenUrl(originalUrl);
            return Ok(new { ShortenedUrl = shortenedUrl });
        }
        catch (Exception ex)
        {
            return BadRequest(new { Error = ex.Message });
        }
    }

    [HttpGet("original")]
    public IActionResult GetOriginalUrl([FromQuery] string shortenedUrl)
    {
        try
        {
            var originalUrl = _urlShortnerService.GetOriginalUrl(shortenedUrl);
            return Ok(new { OriginalUrl = originalUrl });
        }
        catch (Exception ex)
        {
            return NotFound(new { Error = ex.Message });
        }
    }
}
}