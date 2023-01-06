using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public class FirebaseItemsRepository : IItemsRepository
    {
        private readonly string Auth = "8rOiItIG6yhrlnACngoa1gxJmCyQ2SS7U2HO7o0y";
        private readonly string Name = "https://catalogapi-1dfb6-default-rtdb.firebaseio.com/";
        public FirebaseItemsRepository()
        {

        }

        public Task CreateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }

        public Task DeleteItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<Item?> GetItemAsync(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Item>> GetItemsAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateItemAsync(Item item)
        {
            throw new NotImplementedException();
        }
    }
}