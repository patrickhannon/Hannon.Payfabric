using System;
using System.Collections.Generic;
using System.Security.AccessControl;
using System.Threading;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class ResponseModel
    {
        public string Message { get; set; }
        public string Content { get; set; }
        public bool Success { get; set; }
        public string CardName { get; set; }
        public PayFabricResponseModel ResponseValue { get; set; }
        public List<PayfabricCustomerResponseModel> CustomerPaymentDetails { get; set; }
        public PayfabricTransactionResponse TransactionResponse { get; set; }
        public override string ToString()
        {
            return string.Format("Message:{0},Content:{1},Success:{2},WalletId:{3}", Message, Content, Success, ResponseValue);
        }
    }
}
