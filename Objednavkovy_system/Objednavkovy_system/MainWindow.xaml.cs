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
using RestSharp;

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
            var res = client.Execute(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            if (res.Content == "0")
            {
                Error.Content = "Špatné heslo nebo email";
            }
            else
            {
                ShopMain page = new ShopMain();
                page.Show();
                this.Close();
            }
            
        }

        private void goShop_Click(object sender, RoutedEventArgs e)
        {
            ShopMain page = new ShopMain();
            page.Show();
            this.Close();
        }

    }
}
