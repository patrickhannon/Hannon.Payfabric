using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    class TransactionCommand
    {
        public string Url { get; set; }
        public Method HttpVerb { get; set; }
        public string Name { get; set; }
    }

    enum Command
    {
        CreateToken = 1, CreateTrans = 2, ProcessTrans = 3, RetrieveTrans = 4, CancelTrans = 5, PreAuthTrans = 6,
        CreateWallet = 7, UpdateWallet = 8, DeleteWallet = 9, GetCardsByCustomer = 10,
        GetCardByCustomerWallet = 11, CreateAPayLink = 12, UpdateAPayLink = 13,
        RetrieveAPayLink = 14, RemoveAPayLink = 15, CancelAPayLink = 16, ResendPayLinkNotificationEmail = 17
    };
}
