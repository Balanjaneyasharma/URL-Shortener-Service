
using Microsoft.AspNetCore.Mvc;

using MyApi.Domain.Models;
using MyApi.Domain.Interfaces;
using MyApi.Models;
using MyApi.Attributes;

namespace MyApi.Controllers;

[ApiController]
[Route("api/url-short")]
public class UrlShortenerController : ControllerBase
{
    /* 
        [FromBody] [FromRoute] [FromQuery] [FromHeader] [FromForm] 
        these all are called binding source attributes, 
        they are used to specify the source of the data for the action parameters. 
        They help the model binder to understand where to get the data from when binding it to the action parameters.
    */

    private readonly IUrlShortenerService _urlShortenerService;

    public UrlShortenerController(IUrlShortenerService service)
    {
        _urlShortenerService = service;
    }

    [HttpPost("add")]
    public async Task<ActionResult<UrlMapping>> AddShortUrl([FromBody] UrlShortRequestDTO request)
    {
        var shortUrl = await this._urlShortenerService.CreateShortUrl(request.LongUrl);
        return Created(shortUrl, shortUrl);
    }

    [HttpGet("all")]
    public async Task<ActionResult<List<UrlMapping>>> GetAllShortUrls()
    {
        var result = await this._urlShortenerService.GetAllShortUrls();
        return Ok(result);
    }

    [HttpGet("/{shortUrl}")]
    public async Task<IActionResult> GetLongUrl([FromRoute] [ValidShortUrl] string shortUrl)
    {
        var longUrl = await this._urlShortenerService.GetLongUrlByShortUrl(shortUrl);
        return RedirectPermanent(longUrl);
    }

    [HttpDelete("delete/{shortUrl}")]
    public async Task<ActionResult> DeleteShortUrl([FromRoute] [ValidShortUrl] string shortUrl)
    {
        await this._urlShortenerService.DeleteShortUrl(shortUrl);
        return NoContent();
    }
}