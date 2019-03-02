using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    public class PayfabricWalletRequestModel
    {
        /*{
                "SetupId": "Marks_Live",--n
                "Aba": "",
                "Type": "Sale",n
                "Customer": "000000",
                "Amount": 0,n
                "Currency": "USD",
                "CardHolder": {
                                "FirstName": "Patrick",
                                "LastName": "Hannon",
                                "MiddleName": "J"
                },
                "Card": null,n
                "Tender": "CreditCard",
                "Account": null,
                "ExpDate": "0620",
                "Billto": {
                                "Customer": "Patrick Hannon",
                                "Line1": "7303 Heritage Oaks Ct",
                                "City": "Arlington",
                                "State": "TX",
                                "Country": "US",
                                "Zip": "76001",
                                "Email": "patrick_hannon@hotmail.com",
                                "ID": "d46a5b27-06a9-480b-b79b-ff126c575520"
                },
                "Value": null,n
                "Token": "d46a5b27-06a9-480b-b79b-ff126c575520",n
                "Key": null,n
                "Document": null,
                "Shipto": null,n
                "ID": null,
                "IsDefaultCard": true,
                "IsLocked": true,
                "IsSaveCard": true,
                "CardName": "Visa"
            }*/
        public string Aba { get; set; }
        public string Customer { get; set; }
        public string Currency { get; set; }
        public CardHolder CardHolder { get; set; }
        public string Tender { get; set; }
        //public string Account { get; set; }
        public string ExpDate { get; set; }
        public AddressModel Billto { get; set; }
        public Document Document { get; set; }
        public string ID { get; set; }
        public bool IsDefaultCard { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSaveCard { get; set; }
        public string CardName { get; set; }

        public override string ToString()
        {
            return ",Aba:" + Aba + ",Customer:" + Customer + ",Currency:" +
                   ",CardHolder:" + CardHolder + ",Tender:" + Tender + 
                   ",ExpDate:" + ExpDate + ",Billto:" + Billto +
                   ",Document:" + Document + ",ID:" + ID + ",IsDefaultCard:" + IsDefaultCard +
                   ",IsLocked:" + IsLocked + ",IsSaveCard:" + IsSaveCard + ",CardName:" + CardName;
        }
    }
}
