
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Objednavkovy_system
{
    public class Order
    {
       public string id_order { get; set; }

       public DatePicker dateOfOrder { get; set; }

       public int price { get; set; }
       public int state { get; set; }

    }
}
