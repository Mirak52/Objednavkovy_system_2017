using Objednavkovy_system.classes;
using RestSharp;
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

namespace Objednavkovy_system.pages
{
    /// <summary>
    /// Interakční logika pro DetailOrder.xaml
    /// </summary>
    public partial class DetailOrder : Window
    {
        public dynamic detailOrder;
        public Person user = new Person();
        private bool EditMode = false;
        public int Price = 0;
        public DetailOrder(dynamic selectedItem,Person person)
        {
            InitializeComponent();
            user = person;
            detailOrder = selectedItem;
            GetDetails();
        }

        

        private void Hide_Click(object sender, RoutedEventArgs e)
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("action", 2);
            request.AddParameter("id_order", detailOrder.id_order);
            var res = client.Execute<List<Item>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            id_order.Content = "Succesful deleted, just return to orders list";
            Edit.Visibility = Visibility.Hidden;
            LEdit.Visibility = Visibility.Hidden;
            EditMode = false;
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            OrdersPage page = new OrdersPage(user);
            page.Show();
            this.Close();
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            Lupdate.Visibility = Visibility.Visible;
            Return.Visibility = Visibility.Visible;
            Animals.Visibility = Visibility.Visible;
            EditMode = true;
            GetAnimals();
        }
        private void GetAnimals()
        {
            if (EditMode)
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
                Animals.ItemsSource = res.Data;
            }
        }
        private void GetDetails()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.GET);
            request.AddParameter("action", 1);
            request.AddParameter("id_order", detailOrder.id_order);
            var res = client.Execute<List<Item>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = res.Data;
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            id_order.Content = "ID order: " + detailOrder.id_order;
            Order.ItemsSource = queryResult;
        }

        private void Animals_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

            if (EditMode)
            {
                dynamic selectedItem = Animals.SelectedItems[0];
                var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
                var request = new RestRequest(Method.POST);
                request.AddParameter("action", 4);
                request.AddParameter("id_item", selectedItem.id_item);
                request.AddParameter("id_order", detailOrder.id_order);
                var res = client.Execute(request);
                if (res.ResponseStatus == ResponseStatus.Error)
                {
                    throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    //Error.Content= "Chyba na serveru, zkontroluj URL");
                }
                App.IsNumber(detailOrder.price);

                Price = Price + App.IsNumber(selectedItem.price) + App.IsNumber(detailOrder.price);


                client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
                request = new RestRequest(Method.POST);
                request.AddParameter("action", 5);
                request.AddParameter("id_order", detailOrder.id_order);
                request.AddParameter("price", Price);
                res = client.Execute(request);
                if (res.ResponseStatus == ResponseStatus.Error)
                {
                    throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    //Error.Content= "Chyba na serveru, zkontroluj URL");
                }
                GetDetails();
            }
        }

        private void Order_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (EditMode)
            {
                dynamic selectedItem = Order.SelectedItems[0];

                var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
                var request = new RestRequest(Method.POST);
                request.AddParameter("action", 3);
                request.AddParameter("id_itemOrder", selectedItem.id_itemOrder);
                var res = client.Execute(request);
                if (res.ResponseStatus == ResponseStatus.Error)
                {
                    throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    //Error.Content= "Chyba na serveru, zkontroluj URL");
                }
                DetailOrder page = new DetailOrder(detailOrder, user);
                page.Show();
                this.Close();
            }
        }
    }
}
