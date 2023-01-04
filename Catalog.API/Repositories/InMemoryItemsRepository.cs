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

        public IEnumerable<Item> GetItems() => items;

        public Item? GetItem(Guid id) => items.Where(item => item.Id == id).SingleOrDefault();
        public void CreateItem(Item item) => items.Add(item);

        public void UpdateItem(Item item)
        {
            int index = items.FindIndex(existingItem => existingItem.Id == item.Id);
            items[index] = item;
        }

        public void DeleteItem(Guid id)
        {
            int index = items.FindIndex(existingItem => existingItem.Id == id);
            items.RemoveAt(index);
        }
    }
}