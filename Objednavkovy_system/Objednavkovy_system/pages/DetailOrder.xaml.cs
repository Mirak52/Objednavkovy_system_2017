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
        public DetailOrder(dynamic selectedItem,Person person)
        {
            InitializeComponent();
            user = person;
            detailOrder = selectedItem;
            getDetails();
        }

        private void getDetails()
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
            id_order.Content = detailOrder.id_order;
            Order.ItemsSource = queryResult;
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
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            OrdersPage page = new OrdersPage(user);
            page.Show();
            this.Close();
        }
    }
}
