using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public class InMemoryItemsRepository : IItemsRepository
    {
        private readonly List<Item> items = new()
        {
            new Item{ Id=Guid .NewGuid(), Name="Potion", Price=9, CreatedDate=DateTimeOffset.UtcNow },
            new Item{ Id=Guid .NewGuid(), Name="Wooden Sword", Price=19, CreatedDate=DateTimeOffset.UtcNow },
            new Item{ Id=Guid .NewGuid(), Name="Wooden Shield", Price=29, CreatedDate=DateTimeOffset.UtcNow },
        };

        public async Task<IEnumerable<Item>> GetItemsAsync() => await Task.FromResult(items);

        public async Task<Item?> GetItemAsync(Guid id) => await Task.FromResult(items.Where(item => item.Id == id).SingleOrDefault());
        public async Task CreateItemAsync(Item item)
        {
            items.Add(item);
            await Task.CompletedTask;
        }

        public async Task UpdateItemAsync(Item item)
        {
            int index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
            await Task.CompletedTask;
        }

        public async Task DeleteItemAsync(Guid id)
        {
            int index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
            await Task.CompletedTask;
        }
    }
}