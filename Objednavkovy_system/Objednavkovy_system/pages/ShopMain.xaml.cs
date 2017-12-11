using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using RestSharp;
namespace Objednavkovy_system.pages
{
    /// <summary>
    /// Interakční logika pro ShopMain.xaml
    /// </summary>
    public partial class ShopMain : Window
    {
        public ShopMain()
        {
            InitializeComponent();
            getItem();
        }
        public List<ShoppingList> ListOfItems = new List<ShoppingList>();
        public List<Item> listOrder = new List<Item>();
        private void getItem()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
            var request = new RestRequest(Method.GET);
            var res = client.Execute<List<Item>>(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Items.ItemsSource = res.Data;
        }

        private void Items_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = Items.SelectedItems[0];
            ShopList.ItemsSource = "";
            listOrder.Add(new Item(){ id_item = selectedItem.id_item, name= selectedItem.name, price = selectedItem.price, description = selectedItem.description});
            ListOfItems.Add(new ShoppingList(){id_item = selectedItem.id_item, id_order = "0"});
            ShopList.ItemsSource = listOrder;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ShopList.ItemsSource = "";
            listOrder.Clear();
            ShopList.ItemsSource = listOrder;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
          

            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.POST);
            int price =0;
            request.AddParameter("action", 0);
            request.AddParameter("dateOfOrder", "11-12-2017");
            request.AddParameter("DateOfTakeover", "11-12-2017");
            request.AddParameter("price", price);
            request.AddParameter("state", 0);
            var res = client.Execute(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            ShopList.ItemsSource = "";
            listOrder.Clear();
            ShopList.ItemsSource = listOrder;
            saveOrder();
        }

        private void saveOrder()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.POST);
            int price = 0;
            request.AddParameter("action", 1);
            foreach (var order in ListOfItems)
            {
                request.AddParameter("id_item", order.id_item);
                request.AddParameter("id_order", order.id_order);
                var res = client.Execute(request);

                if (res.ResponseStatus == ResponseStatus.Error)
                {
                    throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    //Error.Content= "Chyba na serveru, zkontroluj URL");
                }
            }
            ListOfItems.Clear();
        }
    }
}
