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
using System.Windows.Navigation;
using System.Windows.Shapes;
using Objednavkovy_system.pages;
using Objednavkovy_system.classes;
using RestSharp;
using Newtonsoft.Json;

namespace Objednavkovy_system
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Registration_Click(object sender, RoutedEventArgs e)
        {
            Registration page = new Registration();
            page.Show();
            this.Close();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "" && Password.Password == "")
            {
                Error.Content = "Špatně zadaná data!!!";
                Email.Focus();
            }
            else
            {
                bool emailPassed = true;
                try
                {
                    var eMailValidator = new System.Net.Mail.MailAddress(Email.Text);
                }
                catch (FormatException ex)
                {
                    Error.Content = "Špatně zadaný email!!!";
                    emailPassed = false;
                }
                if (emailPassed)
                {
                    Login();
                }
            }
        }

        private void Login()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("action", 0);
            request.AddParameter("email", Email.Text);
            request.AddParameter("password", Password.Password); 
            var res = client.Execute<List<Person>>(request);   
            if (res.Content == "0")
            {
                Error.Content = "Neznámé heslo nebo email";
            }
            else
            {
                Person osoba = new Person();
                osoba.email = Email.Text;
                osoba.id_person = res.Content;
                ShopMain page = new ShopMain(osoba);
                page.Show();
                this.Close();
            }   
        }
        private void goBack_Click(object sender, RoutedEventArgs e)
        {
            Person osoba = new Person();
            osoba.email = "Guest";
            osoba.id_person = "0";
            ShopMain page = new ShopMain(osoba);
            page.Show();
            this.Close();
        }
    }
}
