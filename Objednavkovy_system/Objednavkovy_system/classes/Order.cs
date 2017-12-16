using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Objednavkovy_system.classes
{
    public class Order
    {
        public string id_order { get; set; }
        public string dateOfOrder { get; set; }
        public string price { get; set; }
        public string state { get; set; }
        public string dateOfTakeover { get; set; }
    }
}
