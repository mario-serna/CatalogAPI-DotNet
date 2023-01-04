using Catalog.API.Entities;

namespace Catalog.API.Repositories
{
    public class FirebaseItemsRepository : IItemsRepository
    {
        private readonly string auth = "8rOiItIG6yhrlnACngoa1gxJmCyQ2SS7U2HO7o0y";
        private readonly string name = "https://catalogapi-1dfb6-default-rtdb.firebaseio.com/";
        public FirebaseItemsRepository()
        {

        }
        public void CreateItem(Item item)
        {
            throw new NotImplementedException();
        }

        public void DeleteItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public Item? GetItem(Guid id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<Item> GetItems()
        {
            throw new NotImplementedException();
        }

        public void UpdateItem(Item item)
        {
            throw new NotImplementedException();
        }
    }
}