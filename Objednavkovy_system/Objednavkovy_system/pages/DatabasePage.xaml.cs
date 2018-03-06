using Objednavkovy_system.classes;
using RestSharp;
using SQLite;
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
    /// Interakční logika pro DatabasePage.xaml
    /// </summary>
    public partial class DatabasePage : Window
    {
        public Person user = new Person();
        public DatabasePage(Person osoba)
        {
            InitializeComponent();
            ShowInformations();
            user = osoba;
        }

        private void ShowInformations()
        {
            var all = App.DatabaseItem.QueryCustom().Result;
            var prices = App.DatabaseItem.ReturnPrices().Result;
            int cena = 0;
            foreach(var price in prices){
                cena = cena + price.price;
            }
           
            Data1.Content = "Celkový součet z lokální databáze: " + cena;
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/item.php?count");
            var request = new RestRequest(Method.GET);
            var res = client.Execute<List>(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };

            Data2.Content = "Celkový součet ze vzdálené databáze: " + res.Content;

        }

        private void Update_Click(object sender, RoutedEventArgs e)
        {

        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            ShopMain page = new ShopMain(user);
            page.Show();
            this.Close();
        }
    }
}
