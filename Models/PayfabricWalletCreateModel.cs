using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hannon.PayFabric.Models;

namespace Hannon.PayFabric.Models
{
    public class PayfabricWalletCreateModel
    {
        public string Tender { get; set; }
        public string Customer { get; set; }
        public string Account { get; set; }
        public string ExpDate { get; set; }
        public string Identifier { get; set; }
        public bool IsDefaultCard { get; set; }
        public AddressModel Billto { get; set; }
        public CardHolder CardHolder { get; set; }
    }
}
