using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class Shipto
    {
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public string Country { get; set; }
        public string PhoneNumber { get; set; }

        public override string ToString()
        {
            return string.Format("Line1:{0},City:{1},State:{2},Country:{3},Zip:{4}",
               Line1, City, State, Country, Zip);
        }
    }
}
