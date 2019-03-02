using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hannon.PayFabric.Models;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class PayfabricCustomerResponseModel
    {
        public string Aba { get; set; }
        public string Account { get; set; }
        public string AccountType { get; set; }
        public Billto Billto { get; set; }
        public CardHolder CardHolder { get; set; }
        public string CardName { get; set; }
        public object CheckNumber { get; set; }
        public string Connector { get; set; }
        public string Customer { get; set; }
        public string ExpDate { get; set; }
        public string GPAddressCode { get; set; }
        public string GatewayToken { get; set; }
        public string ID { get; set; }
        public string Identifier { get; set; }
        public bool IsDefaultCard { get; set; }
        public bool IsLocked { get; set; }
        public bool IsSaveCard { get; set; }
        public string IssueNumber { get; set; }
        public string ModifiedOn { get; set; }
        public string StartDate { get; set; }
        public string Tender { get; set; }
        public string UserDefine1 { get; set; }
        public string UserDefine2 { get; set; }
        public string UserDefine3 { get; set; }
        public string UserDefine4 { get; set; }

        public override string ToString()
        {
            return string.Format("Aba:{0}Account: {1},AccountType: {2},Billto: {3},CardHolder: {4},CardName: {5},CheckNumber: {6}"+
            ",Connector:{7},Customer:{8},ExpDate:{9},GPAddressCode:{10},GatewayToken:{11},ID: {12},Identifier:{13},IsDefaultCard:{14}"+
            "IsLocked:{15},IsSaveCard: {16},IssueNumber:{17},ModifiedOn:{18},StartDate:{19},Tender:{20},UserDefine1:{21},"+
            "UserDefine2:{22},UserDefine3: {23},UserDefine4:{24}", Aba, Account, AccountType, Billto, CardHolder, CardName,
            CheckNumber,Connector, Customer, ExpDate, GPAddressCode, GatewayToken, ID, Identifier, IsDefaultCard, IsLocked,
            IsSaveCard, IssueNumber, ModifiedOn, StartDate, Tender, UserDefine1, UserDefine2, UserDefine3, UserDefine4);

        }
    }
}
