using Catalog.API.Dtos;
using Catalog.API.Entities;
using Catalog.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.API.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository repository;
    private readonly ILogger<ItemsController> logger;

    public ItemsController(IItemsRepository repository, ILogger<ItemsController> logger)
    {
        this.repository = repository;
        this.logger = logger;
    }

    [HttpGet]
    public async Task<IEnumerable<ItemDto>?> GetItemsAsync()
    {
        var items = await repository.GetItemsAsync();
        if (items is null) return null;
        logger.LogInformation($"{DateTime.UtcNow.ToString("s")}: Retrieved {items.Count()} items");
        return items.Select(item => item.AsDto());
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<ItemDto>> GetItemAsync(Guid id)
    {
        var items = await repository.GetItemAsync(id);
        if (items == null) return NotFound();
        return items.AsDto();
    }

    [HttpPost]
    public async Task<ActionResult<ItemDto>> CreateItemAsync(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Description = itemDto.Description,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        await repository.CreateItemAsync(item);

        // CreatedAtAction gives the item url inside of location header
        // Example:
        // content-type: application/json; charset=utf-8 
        // date: Tue,03 Jan 2023 18:50:49 GMT 
        // location: https://localhost:7201/Items/0a45b716-1fb5-4c33-a574-14095c3a476c 
        // server: Kestrel 
        return CreatedAtAction(nameof(GetItemAsync), new { Id = item.Id }, item.AsDto());
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ItemDto>> UpdateItemAsync(Guid id, UpdateItemDto itemDto)
    {
        Item? existingItem = await repository.GetItemAsync(id);
        if (existingItem is null) return NotFound();

        existingItem.Name = itemDto.Name;
        existingItem.Price = itemDto.Price;

        await repository.UpdateItemAsync(existingItem);

        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ItemDto>> DeleteItemAsync(Guid id)
    {
        Item? existingItem = await repository.GetItemAsync(id);
        if (existingItem is null) return NotFound();

        await repository.DeleteItemAsync(existingItem.Id);

        return NoContent();
    }

}
