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
    public partial class ShopMain : Window
    {
        public ShopMain(Person osoba)
        {
            InitializeComponent();
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

    }
}
