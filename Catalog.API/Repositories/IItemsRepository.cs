using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public interface IItemsRepository
    {
        Task<Item?> GetItemAsync(Guid id);
        Task<IEnumerable<Item>> GetItemsAsync();
        Task CreateItemAsync(Item item);
        Task UpdateItemAsync(Item item);
        Task DeleteItemAsync(Guid id);
    }
}