using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class Billto
    {
        public string City { get; set; }
        public string Country { get; set; }
        public string Customer { get; set; }
        public string Email { get; set; }
        public string ID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string ModifiedOn { get; set; }
        public string Phone { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
    }
}
