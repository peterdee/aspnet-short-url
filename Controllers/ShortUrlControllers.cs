using Microsoft.AspNetCore.Mvc;
using aspnet_short_url.Models;
using aspnet_short_url.Services;

namespace aspnet_short_url.Controllers;

[ApiController]
// TODO: proper routing
[Route("api/s")]
public class ShortUrlController(ShortUrlService service) : ControllerBase
{
  private readonly ShortUrlService _service = service;

  [HttpGet]
  public IActionResult Get()
  {
    return Ok();
  }

  [HttpGet("{id:length(24)}")]
  public async Task<ActionResult<ShortUrl>> Get(string id)
  {
    var record = await _service.FindOneByShortIdAsync(id);
    if (record is null)
    {
      return NotFound();
    }

    return record;
  }

  [HttpPost]
  public async Task<IActionResult> Post(ShortUrl value)
  {
    await _service.InsertOneAsync(value);

    return CreatedAtAction(nameof(Get), new { id = value.ShortId }, value);
  }

  [HttpPut("{id:length(24)}")]
  public async Task<IActionResult> Update(string id, ShortUrl value)
  {
    var record = await _service.FindOneByShortIdAsync(id);

    if (record is null)
    {
      return NotFound();
    }

    value.Id = record.Id;

    await _service.UpdateOneByShortIdAsync(id, value);

    return NoContent();
  }

  [HttpDelete("{id:length(24)}")]
  public async Task<IActionResult> Delete(string id)
  {
    var record = await _service.FindOneByShortIdAsync(id);

    if (record is null)
    {
      return NotFound();
    }

    await _service.DeleteOneByShortIdAsync(id);

    return NoContent();
  }
}
