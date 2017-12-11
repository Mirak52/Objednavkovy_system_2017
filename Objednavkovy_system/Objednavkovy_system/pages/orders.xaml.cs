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
    /// Interaction logic for orders.xaml
    /// </summary>
    public partial class orders : Window
    {
        public orders()
        {
            InitializeComponent();
            getOrders();
        }

        private void getOrders()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php");
            var request = new RestRequest(Method.GET);
            var res = client.Execute<List<Order>>(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Orders.ItemsSource = res.Data;
        }
    }
}
