using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class CardHolder
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public override string ToString()
        {
            return "FirstName=" + FirstName + ",MiddleName" + MiddleName + ",LastName" + LastName;
        }
    }
}
