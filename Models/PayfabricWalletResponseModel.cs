using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace Hannon.PayFabric.Models
{
    public class PayfabricWalletResponseModel
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
