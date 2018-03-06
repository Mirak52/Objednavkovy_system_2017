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
        public List<Person> userData = new List<Person>();
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
           
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
            var request = new RestRequest(Method.GET);
            request.AddParameter("action", 0);
            request.AddParameter("id_person", Person.id_person);
            var res = client.Execute(request);
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
            }
            numberOfOrders.Content = res.Content;
            client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
            request = new RestRequest(Method.GET);
            
            request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
            
            request.AddParameter("action", 2);
            request.AddParameter("id_person", Person.id_person);
            var response = client.Execute<List<Person>>(request);
            userData = response.Data;
            foreach(var data in userData)
            {
                Person.name = data.name;
                Person.sirName = data.sirName;
            }
            name.Content = "Name: " + Person.name;
            sirName.Content = "Sir name: " + Person.sirName;
        }

        private void changeEmail_Click(object sender, RoutedEventArgs e)
        {
            HideEntry();
            LColum_1.Visibility = Visibility.Visible;
            Colum_1.Visibility = Visibility.Visible;
            Send.Visibility = Visibility.Visible;
            LColum_1.Content = "Enter new mail";
            action = 1;
        }

        private void changeName_Click(object sender, RoutedEventArgs e)
        {
            HideEntry();
            LColum_1.Visibility = Visibility.Visible;
            Colum_1.Visibility = Visibility.Visible;
            LColum_2.Visibility = Visibility.Visible;
            Colum_2.Visibility = Visibility.Visible;
            Send.Visibility = Visibility.Visible;
            LColum_1.Content = "Enter your name";
            LColum_2.Content = "Enter your sir name";
            action = 2;
        }

        private void changePassword_Click(object sender, RoutedEventArgs e)
        {
            HideEntry();
            LColum_1.Visibility = Visibility.Visible;
            Colum_1.Visibility = Visibility.Visible;
            LColum_2.Visibility = Visibility.Visible;
            Colum_2.Visibility = Visibility.Visible;
            Send.Visibility = Visibility.Visible;
            LColum_1.Content = "Enter new password";
            LColum_2.Content = "Repeate";
            action = 3;
        }

        private void Send_Click(object sender, RoutedEventArgs e)
        {
            if(action == 1) {
                bool emailPassed = true;
                try
                {
                    var eMailValidator = new System.Net.Mail.MailAddress(Colum_1.Text);
                }
                catch (FormatException ex)
                {
                    Response.Content = "Špatně zadaný email!!!";
                    emailPassed = false;
                }
                if (emailPassed)
                {
                    var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("action", 2);
                    request.AddParameter("email", Colum_1.Text);
                    request.AddParameter("id_person", Person.id_person);
                    var res = client.Execute(request);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                    if (res.ResponseStatus == ResponseStatus.Error)
                    {
                        throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    }
                    Response.Content = "Uspěšně updatnuto";
                    Person.email = Colum_1.Text;
                }

            }
            if (action == 2)
            {
                if (Colum_1.Text != "" & Colum_2.Text != "")
                {
                    var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("action", 3);
                    request.AddParameter("name", Colum_1.Text);
                    request.AddParameter("sirName", Colum_2.Text);
                    request.AddParameter("id_person", Person.id_person);
                    var res = client.Execute(request);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                    if (res.ResponseStatus == ResponseStatus.Error)
                    {
                        throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    }
                    Person.name = Colum_1.Text;
                    Person.sirName = Colum_2.Text;
                    Response.Content = "Uspěšně updatnuto";
                    request.AddParameter("name", Colum_1.Text);
                }
                else
                {
                    Response.Content = "Enter both entry";
                }
            }
            if (action == 3) {
                if (Colum_1.Text != "" & Colum_2.Text != "" & Colum_1.Text == Colum_2.Text)
                {
                    var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
                    var request = new RestRequest(Method.POST);
                    request.AddParameter("action", 4);
                    request.AddParameter("password", Colum_1.Text);
                    request.AddParameter("id_person", Person.id_person);
                    var res = client.Execute(request);
                    request.OnBeforeDeserialization = resp => { resp.ContentType = "application/json"; };
                    if (res.ResponseStatus == ResponseStatus.Error)
                    {
                        throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                    }
                    Response.Content = "Uspěšně updatnuto";
                    request.AddParameter("name", Colum_1.Text);
                }
                else
                {
                    Response.Content = "Enter both entry";
                }
            }
            HideEntry();
            GetAccountData();
        }
        private void HideEntry()
        {
            LColum_1.Visibility = Visibility.Hidden;
            Colum_1.Visibility = Visibility.Hidden;
            LColum_2.Visibility = Visibility.Hidden;
            Colum_2.Visibility = Visibility.Hidden;
            Send.Visibility = Visibility.Hidden;
            Colum_1.Text = "";
            Colum_2.Text = "";

        }

        private void return_Click(object sender, RoutedEventArgs e)
        {
            ShopMain page = new ShopMain(Person);
            page.Show();
            this.Close();
        }
    }
}
