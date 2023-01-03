using Catalog.Dtos;
using Catalog.Entities;
using Catalog.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Catalog.Controllers;

[ApiController]
[Route("[controller]")]
public class ItemsController : ControllerBase
{
    private readonly IItemsRepository? repository;

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

}
