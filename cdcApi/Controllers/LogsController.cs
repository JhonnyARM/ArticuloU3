using Microsoft.AspNetCore.Mvc;
using cdcApi.Models;
using cdcApi.Services;
using MongoDB.Bson;

namespace cdcApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LogsController : ControllerBase
{
  private readonly ItemService _itemService;


  public LogsController(ItemService itemService) =>
      _itemService = itemService;

  [HttpGet]
  public async Task<List<Item>> Get() =>
      await _itemService.GetAsync();

  // Endpoint para obtener logs desde MongoDB
  [HttpGet("mongo")]
  public async Task<List<AfterData>> GetMongo()
  {
    // Llamada al método para obtener datos de MongoDB
    return await _itemService.GetAfter();
  }


  [HttpGet("mongoraw")]
  public async Task<List<BsonDocument>> GetRawJson()
  {
    // Llamada al método para obtener datos crudos de MongoDB
    return await _itemService.GetBsonDocument();
  }




  [HttpGet("{id}")]
  public async Task<ActionResult<Item>> Get(string id)
  {
    var item = await _itemService.GetAsync(id);

    if (item is null)
    {
      return NotFound();
    }

    return item;
  }

  [HttpPost]
  public async Task<IActionResult> Post(Item newItem)
  {
    await _itemService.CreateAsync(newItem);
    return CreatedAtAction(nameof(Get), new { id = newItem.Id }, newItem);
  }

  [HttpPut("{id}")]
  public async Task<IActionResult> Update(string id, Item updatedItem)
  {
    var item = await _itemService.GetAsync(id);

    if (item is null)
    {
      return NotFound();
    }

    updatedItem.Id = item.Id;

    await _itemService.UpdateAsync(id, updatedItem);

    return NoContent();
  }

  [HttpDelete("{id}")]
  public async Task<IActionResult> Delete(string id)
  {
    var item = await _itemService.GetAsync(id);

    if (item is null)
    {
      return NotFound();
    }

    await _itemService.RemoveAsync(id);

    return NoContent();
  }
}
