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
    /// Interakční logika pro ordersPage.xaml
    /// </summary>
    public partial class OrdersPage : Window
    {
        public Person user = new Person();
        public OrdersPage(Person osoba)
        {
            InitializeComponent();
            user = osoba;
            getOrders();
        }

        private void getOrders()
        {

            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/order.php");
            var request = new RestRequest(Method.GET);
            request.AddParameter("action", 0);
            request.AddParameter("id_person", user.id_person);
            var res = client.Execute<List<Order>>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            var queryResult = res.Data;
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Orders.ItemsSource = queryResult;
        }

        private void Orders_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            dynamic selectedItem = Orders.SelectedItems[0];
            DetailOrder page = new DetailOrder(selectedItem,user);
            page.Show();
            this.Close();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            ShopMain page = new ShopMain(user);
            page.Show();
            this.Close();
        }
    }
}
