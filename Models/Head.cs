using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class Head
    {
        //public string OrderDate { get; set; }
        //{ "Name": "InvoiceNumber", "Value": "INV14-98870" },
        public string Name { get; set; }
        public string Value { get; set; }

        public override string ToString()
        {
            return "Name:"+Name+",Value:"+Value;
        }
    }
}
