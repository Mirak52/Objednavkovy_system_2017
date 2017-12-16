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
using Objednavkovy_system.classes;
using RestSharp;

namespace Objednavkovy_system.pages
{
    /// <summary>
    /// Interakční logika pro ShopMain.xaml
    /// </summary>
    /// 
    
    public partial class ShopMain : Window
    {
        public List<itemOrder> ItemOrder = new List<itemOrder>();
        public List<shoppingList> ShoppingList = new List<shoppingList>();
        public int price = 0;
        public string id_person;
        public string order;
        public ShopMain(Person osoba)
        {
            InitializeComponent();
            id_person = osoba.id_person;
            if(osoba.email== "Guest")
            {
                //Buy.Visibility = Visibility.Hidden;
                //Clear.Visibility = Visibility.Hidden;
            }
            GetAnimals();
        }
        private void GetAnimals()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
            var request = new RestRequest(Method.GET);
            var res = client.Execute<List<Item>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = res.Data;
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Animals.ItemsSource = queryResult;

        }
        private void GetAnimalsByName(string name)
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
            var request = new RestRequest(Method.GET);
            request.AddParameter("name", name);
            var res = client.Execute<List<Item>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = res.Data;
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Animals.ItemsSource = queryResult;
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            Default.Visibility = Visibility.Hidden;
            GetAnimals();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Default.Visibility= Visibility.Visible;
            GetAnimalsByName(nameSearch.Text);
        }

        private void Animals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            
            dynamic selectedItem = Animals.SelectedItems[0];
            ShopList.ItemsSource = "";
            price = price + App.IsNumber(selectedItem.price);
            ShoppingList.Add(new shoppingList() {name = selectedItem.name, price = App.IsNumber(selectedItem.price)});
            ItemOrder.Add(new itemOrder() { id_item = selectedItem.id_item});
            totalPrice.Content = price;
            ShopList.ItemsSource = ShoppingList;
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearLists();
        }

        private void ClearLists()
        {
            price = 0;
            ShopList.ItemsSource = "";
            ItemOrder.Clear();
            ShoppingList.Clear();
            totalPrice.Content = price;
            ShopList.ItemsSource = ShoppingList;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("action", 0);
            request.AddParameter("price", price);
            request.AddParameter("id_person", id_person);
            var res = client.Execute(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
           
            foreach (var Data in ItemOrder)
            {
                order = order + "('" + Data.id_item + "','" + res.Content + "'),";

            }
            order = order.Remove(order.Length - 1);
            client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            request = new RestRequest(Method.POST);
            request.AddParameter("action", 1);
            request.AddParameter("data", order);
            res = client.Execute(request);
           
            if(res.Content == "1")
            {
                ClearLists();
                totalPrice.Content = "Objednávka uspěšně vytvořena!!";
            }
            else
            {
                totalPrice.Content = "Soudruzi z NDR udělali někde chybu!!";
            }

        }
        
    }
}
