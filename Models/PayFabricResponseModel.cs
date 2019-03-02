using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hannon.PayFabric.Models;

namespace Hannon.PayFabric.Models
{ 
    [Serializable]
    public class PayFabricResponseModel
    {
        public string Message { get; set; }
        public string Result { get; set; }
        public string Key { get; set; }
        public string Token { get; set; }
        public List<PayfabricCustomerResponseModel> CustomerPaymentDetails { get; set; }

        public string StatusCode { get; set; }
        public string ResponseStatus { get; set; }
        public string ResponseUri { get; set; }
        public override string ToString()
        {
            PayfabricCustomerResponseModel model = new PayfabricCustomerResponseModel();
            if (CustomerPaymentDetails != null && CustomerPaymentDetails.Count > 0)
            {
                model = CustomerPaymentDetails.FirstOrDefault();
            }
            return string.Format("Message:{0},Result:{1},Key:{2},Token:{3},CustomerPaymentDetails:{4},", model.ToString());
        }
    }
}
