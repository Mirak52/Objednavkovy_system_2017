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
        public string order;
        public List<Item> listView = new List<Item>();
        public Person user = new Person();
        public ShopMain(Person osoba)
        {
            InitializeComponent();
            user = osoba;
            User.Content = osoba.email;
            if(user.email== "Guest")
            {
                Buy.Visibility = Visibility.Hidden;
                Clear.Visibility = Visibility.Hidden;
                showAccount.Visibility = Visibility.Hidden;
                showOrders.Visibility = Visibility.Hidden;
                totalPrice.Content = "Guest si nemůže objednat položky";
            }
            GetAnimals();
        }
        private void GetAnimals()
        {
            var Items = App.DatabaseItem.QueryCustom().Result;
            if(Items.Count != 0)
            {
                if (App.CheckForInternetConnection())
                {
                    totalPrice.Content = "Bez internetu nemůžeš vytvářet objednávky";
                }
                foreach (var item in Items)
                {
                    Animals.Items.Add(item);
                }
            }
            else if(App.CheckForInternetConnection()){
                App.saveItemsToDatabase();
                GetAnimals();
            }
            else
            {
                totalPrice.Content = "Nemáš přístup k internetu a v databázi nic nemáš!";
            }
        }

        private void GetAnimalsByName(string name)
        {
            Animals.Items.Clear();
            var searchedData = App.DatabaseItem.SelectByName(name).Result;
            foreach (var item in searchedData)
            {
                Animals.Items.Add(item);
            }
            Search.Visibility = Visibility.Hidden;

            /* var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
             var request = new RestRequest(Method.GET);
             request.AddParameter("name", name);
             var res = client.Execute<List<Item>>(request);
             request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
             listView = res.Data;
             Animals.Items.Clear();
             if (res.ResponseStatus == ResponseStatus.Error)
             {
                 throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                 //Error.Content= "Chyba na serveru, zkontroluj URL");
             }
             foreach(var item in res.Data)
             {
                 Animals.Items.Add(item);
             }
             Search.Visibility = Visibility.Hidden;
         */
        }

        private void Default_Click(object sender, RoutedEventArgs e)
        {
            Default.Visibility = Visibility.Hidden;
            Search.Visibility = Visibility.Visible;
            GetAnimals();
        }

        private void Search_Click(object sender, RoutedEventArgs e)
        {
            Default.Visibility= Visibility.Visible;
            GetAnimalsByName(nameSearch.Text);
        }


        private void Animals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (user.email == "Guest")
            {
                Buy.Visibility = Visibility.Hidden;
                Clear.Visibility = Visibility.Hidden;
                showAccount.Visibility = Visibility.Hidden;
                showOrders.Visibility = Visibility.Hidden;
                totalPrice.Content = "Nepříhlášený uživatel si nemůže objednat položky";
            }
            else {
                if (App.CheckForInternetConnection())
                {
                    Buy.Visibility = Visibility.Visible;
                    Clear.Visibility = Visibility.Visible;
                }
                else
                {
                    totalPrice.Content = "Bez internetu nevytvoříš objednávku";
                }
            }
            dynamic selectedItem = Animals.SelectedItems[0];
            ShopList.ItemsSource = "";
            price = price + selectedItem.price;

            ShoppingList.Add(new shoppingList() {name = selectedItem.name, price = selectedItem.price});
            ItemOrder.Add(new itemOrder() { id_item = selectedItem.id_item});
            totalPrice.Content = price;
            ShopList.ItemsSource = ShoppingList;
            //Animals.Items.Clear();
        }

        private void Clear_Click(object sender, RoutedEventArgs e)
        {
            ClearLists();
            totalPrice.Content = "Succesfull cleared";
        }

        private void ClearLists()
        {
            price = 0;
            ShopList.ItemsSource = "";
            ItemOrder.Clear();
            ShoppingList.Clear();
            totalPrice.Content = price;
            ShopList.ItemsSource = ShoppingList;
            Buy.Visibility = Visibility.Hidden;
            Clear.Visibility = Visibility.Hidden;
        }

        private void Buy_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("action", 0);
            request.AddParameter("price", price);
            request.AddParameter("id_person", user.id_person);
            var res = client.Execute(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
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
            Buy.Visibility = Visibility.Hidden;
            Clear.Visibility = Visibility.Hidden;

        }

        private void showOrders_Click(object sender, RoutedEventArgs e)
        {
            OrdersPage page = new OrdersPage(user);
            page.Show();
            this.Close();
        }

        private void showAccount_Click(object sender, RoutedEventArgs e)
        {
            AccountPage page = new AccountPage(user);
            page.Show();
            this.Close();
        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow page = new MainWindow();
            page.Show();
            this.Close();
        }

        private void Database_Click(object sender, RoutedEventArgs e)
        {
            DatabasePage page = new DatabasePage(user);
            page.Show();
            this.Close();
        }
    }
}
