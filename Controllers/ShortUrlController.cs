namespace aspnet_short_url.Controllers;

using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using aspnet_short_url.Models;
using aspnet_short_url.Services;
using aspnet_short_url.Utilities;

[ApiController]
[Route("api/url")]
public class ShortUrlController(ShortUrlService service) : ControllerBase
{
  private readonly ShortUrlService _service = service;

  [HttpGet("{id}")]
  public async Task<ActionResult> Get(string id)
  {
    var record = await _service.FindOneByShortIdAndUpdateAsync(
      id,
      new BsonDocument("$inc", new BsonDocument("RedirectCount", 1))
    );
    if (record is null)
    {
      return NotFound();
    }

    return Redirect(record.OriginalUrl);
  }

  [HttpGet("{id}/info")]
  public async Task<ActionResult> GetInfo(string id)
  {
    var record = await _service.FindOneByShortIdAndUpdateAsync(
      id,
      new BsonDocument("$inc", new BsonDocument("RedirectCount", 1))
    );
    if (record is null)
    {
      return NotFound();
    }

    record.RedirectCount += 1;
    return Ok(record);
  }

  [HttpPost]
  public async Task<IActionResult> Post([FromForm] ShortUrlPostDto value)
  {
    if (!UtilityFunctions.IsValidUrl(value.Url))
    {
      return BadRequest();
    }

    var newRecord = new ShortUrl()
    {
      OriginalUrl = value.Url,
      RedirectCount = 0,
      ShortId = UtilityFunctions.GenerateId(),
    };
    await _service.InsertOneAsync(newRecord);

    return CreatedAtAction(nameof(Get), new { id = newRecord.ShortId }, newRecord);
  }

  [HttpPut("{id}")]
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

  [HttpDelete("{id}")]
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
