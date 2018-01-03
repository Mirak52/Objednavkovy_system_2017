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
    /// Interaction logic for Registration.xaml
    /// </summary>
    public partial class Registration : Window
    {
        public Registration()
        {
            InitializeComponent();
        }

        private void Return_Click(object sender, RoutedEventArgs e)
        {
            MainWindow page = new MainWindow();
            page.Show();
            this.Close();
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            if (Email.Text == "" && Password.Password == "" && PasswordAgain.Password == "" && Password.Password == PasswordAgain.Password)
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
                    Registrate();
                }
            }
        }
        private void Registrate()
        {
            var client = new RestClient("https://student.sps-prosek.cz/~bastlma14/obj/account.php");
            var request = new RestRequest(Method.POST);
            request.AddParameter("action", 1);
            request.AddParameter("email", Email.Text);
            request.AddParameter("password", Password.Password);
            request.AddParameter("name", Name.Text);
            request.AddParameter("sirName", sirName.Text);
            var res = client.Execute(request);
            if (res.ResponseStatus == ResponseStatus.Error)
            {
                throw new System.ArgumentException("Chyba na serveru, zkontroluj URL");
                //Error.Content= "Chyba na serveru, zkontroluj URL");
            }
            Error.Content = "Úspěšně registrován";
        }
    }
}
