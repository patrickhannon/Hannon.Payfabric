using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]    
    public class PayfabricTransactionResponse
        {
            public string AVSAddressResponse { get; set; }
            public string AVSZipResponse { get; set; }
            public string AuthCode { get; set; }
            public string CVV2Response { get; set; }
            public string IAVSAddressResponse { get; set; }
            public string Message { get; set; }
            public string OriginationID { get; set; }
            public string PayFabricErrorCode { get; set; }
            public string RespTrxTag { get; set; }
            public string ResultCode { get; set; }
            public string Status { get; set; }
            public string TAXml { get; set; }
            public string TrxDate { get; set; }
            public string TrxKey { get; set; }

            public override string ToString()
            {
                return string.Format("AVSAddressResponse:{0},AVSZipResponse:{1},AuthCode:{2}," +
                    "CVV2Response:{3},IAVSAddressResponse:{4},Message:{5},OriginationID:{6},PayFabricErrorCode:{7},"+
                    "RespTrxTag:{8},ResultCode:{9},Status:{10},TAXml:{11},TrxDate:{12},TrxKey:{13}",
                    AVSAddressResponse, AVSZipResponse, AuthCode, CVV2Response, IAVSAddressResponse,
                    Message, OriginationID, PayFabricErrorCode, RespTrxTag, ResultCode, Status, TAXml, TrxDate,
                    TrxKey);
            }
        }
       
}
