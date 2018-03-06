using Objednavkovy_system.classes;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows;

namespace Objednavkovy_system
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int IsNumber(string number)
        {
            int x = 0;
            Int32.TryParse(number, out x);
            return x;
        }
        public static ItemDatabase _Item;

        public static ItemDatabase DatabaseItem
        {
            get
            {
                if (_Item == null)
                {
                    var fileHelper = new FileHelper();
                    _Item = new ItemDatabase(fileHelper.GetLocalFilePath("TodoSQLite.db3"));
                }
                return _Item;
            }
        }
        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                {
                    using (client.OpenRead("http://clients3.google.com/generate_204"))
                    {
                        return true;
                    }
                }
            }
            catch
            {
                return false;
            }
        }
        internal static void saveItemsToDatabase()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
            var request = new RestRequest(Method.GET);
            var res = client.Execute<List<Item>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
       
            Task task = new Task(() => {
                saveData(res.Data);
            });
            task.Start();
            task.Wait();
        }

        private static void saveData(List<Item> data)
        {
            foreach (var item in data)
            {
                Item itemToDatabase = new Item();
                itemToDatabase.id_item = item.id_item;
                itemToDatabase.name = item.name;
                itemToDatabase.price = item.price;
                itemToDatabase.description = item.description;
                App.DatabaseItem.SaveItemAsync(itemToDatabase);
            }
        }

        public static void deleteAllItems()
        {
            App.DatabaseItem.DeleteAll();
        }

    }


}
