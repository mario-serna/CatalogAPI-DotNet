using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository repository;

    public ItemsController(IItemsRepository repository)
    {
        this.repository = repository;
    }

    [HttpGet]
    public IEnumerable<ItemDto>? GetItems() => repository?.GetItems().Select(item => item.AsDto());

    [HttpGet("{id}")]
    public ActionResult<ItemDto> GetItem(Guid id)
    {
        var items = repository?.GetItem(id);
        if (items == null) return NotFound();
        return items.AsDto();
    }

    [HttpPost]
    public ActionResult<ItemDto> CreateItem(CreateItemDto itemDto)
    {
        Item item = new()
        {
            Id = Guid.NewGuid(),
            Name = itemDto.Name,
            Price = itemDto.Price,
            CreatedDate = DateTimeOffset.UtcNow
        };

        repository.CreateItem(item);

        // CreatedAtAction gives the item url inside of location header
        // Example:
        // content-type: application/json; charset=utf-8 
        // date: Tue,03 Jan 2023 18:50:49 GMT 
        // location: https://localhost:7201/Items/0a45b716-1fb5-4c33-a574-14095c3a476c 
        // server: Kestrel 
        return CreatedAtAction(nameof(GetItem), new { Id = item.Id }, item.AsDto());
    }

}
