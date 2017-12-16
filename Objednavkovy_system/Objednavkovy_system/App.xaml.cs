using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Objednavkovy_system
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public static int IsNumber(string number)
        {
            int x = 0;
            Int32.TryParse(number, out x);
            return x;
        }
    }
   
}
