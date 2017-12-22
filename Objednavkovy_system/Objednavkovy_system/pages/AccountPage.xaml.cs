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
    /// Interakční logika pro AccountPage.xaml
    /// </summary>
    public partial class AccountPage : Window
    {
        public Person Person = new Person();
        public int action = 0;
        public AccountPage(Person user)
        {
            InitializeComponent();
            Person = user;
            GetAccountData();
        }

        private void GetAccountData()
        {
            UserName.Content = Person.email;
            mail.Content = "Email: " + Person.email;
            name.Content = "Name: " + Person.name;
            sirName.Content = "Sir name: " + Person.sirName;
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
            var request = new RestRequest(Method.GET);
            request.AddParameter("action", 0);
            request.AddParameter("id_person", Person.id_person);
            var res = client.Execute(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            numberOfOrders.Content = res.Content;
        }
    }
}
