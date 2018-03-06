using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objednavkovy_system.classes
{
    public class ItemDatabase
    {
        public SQLiteAsyncConnection database;

        public ItemDatabase(string dbPath)
        {
            database = new SQLiteAsyncConnection(dbPath);
            database.CreateTableAsync<Item>().Wait();
        }
        public Task<List<Item>> QueryCustomExist(string name)
        {
            return database.QueryAsync<Item>("select ID FROM [Item] where Name ='" + name + "'");
        }
        public Task<List<Item>> SelectByName(string name)
        {
            return database.QueryAsync<Item>("select * FROM [Item] where name  LIKE '%" + name+ "%'");
        }
        public Task<List<Item>> QueryCustom()
        {
            return database.QueryAsync<Item>("select * FROM [Item]");
        }
        public Task<List<Item>> Add()
        {
            return database.QueryAsync<Item>("INSERT INTO [Item] (Name,Text) VALUES (`Ahoj`,`Pepa`)");
        }
        public Task<List<Item>> DeleteAll()
        {
            return database.QueryAsync<Item>("Delete FROM [Item]");
        }
        public Task<List<Item>> ReturnPrices()
        {
            return database.QueryAsync<Item>("SELECT price FROM [Item]");
        }
        // Query
        public Task<List<Item>> GetItemsAsync()
        {
            return database.Table<Item>().ToListAsync();
        }

        // Query using SQL query string
        public Task<List<Item>> GetItemsNotDoneAsync()
        {
            return database.QueryAsync<Item>("SELECT * FROM [Item] WHERE [Done] = 0");
        }

        // Query using LINQ
     

        public Task<int> SaveItemAsync(Item item)
        {
                return database.InsertAsync(item);
        }

        public Task<int> DeleteItemAsync(Item item)
        {
            return database.DeleteAsync(item);
        }
    }
}
