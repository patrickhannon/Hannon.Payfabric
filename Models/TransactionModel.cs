using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class TransactionModel
    {
        public string SetupId { get; set; }
        public string Aba { get; set; }
        public string Type { get; set; }
        public string Customer { get; set; }
        public Decimal Amount { get; set; }
        public string Currency { get; set; }
        public CardHolder CardHolder { get; set; }
        public Card Card { get; set; }
        public string Tender { get; set; }
        public string Account { get; set; }
        public string ExpDate { get; set; }
        public AddressModel Billto { get; set; }
        public string Value { get; set; }
        public string Token { get; set; }
        public string Key { get; set; }
        public Document Document { get; set; }
        public Shipto Shipto { get; set; }
        public string ID { get; set; }
        public bool IsDefaultCard { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSaveCard { get; set; }
        public string CardName { get; set; }
        public override string ToString()
        {
            return "Setup:" + SetupId + ",Type:" + Type + ",Amount:" + Amount + ",Currency:" +
                   ",CardHolder:" + CardHolder + ",Card:" + Card + ",Tender:" + Tender + ",Account:" + Account +
                   ",ExpDate:" + ExpDate + ",Billto:" + Billto + ",Value:" + Value + ",Token:" + Token + ",Key:" + Key +
                   ",Document:" + Document + ",Shipto" + Shipto;
        }

    }
}
