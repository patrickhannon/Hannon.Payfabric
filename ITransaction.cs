using Hannon.PayFabric.Models;

namespace Hannon.PayFabric
{

    public interface ITransaction
    {
        PayFabricResponseModel CreateSecurityToken();
        ResponseModel CreateSecurityToken(string deviceId, string payfabricGatewayAccountProfile, string payfabricUrl);
        ResponseModel CreateTrans(TransactionModel model);
        ResponseModel CancelTrans(TransactionModel model);
        ResponseModel ProcessTransaction(TransactionModel model);
        ResponseModel RetrieveTransaction(TransactionModel model);
        ResponseModel CancelTransaction(TransactionModel model);
        ResponseModel CreatePreAuthTrans(TransactionModel model);
        ResponseModel CreateWallet(PayfabricWalletCreateModel model);
        ResponseModel UpdateWallet(PayfabricWalletRequestModel model);
        ResponseModel DeleteWallet(TransactionModel model);
        ResponseModel GetCardsByCustomer(TransactionModel model);
        ResponseModel GetCardByCustomerWallet(TransactionModel model);
        void TestTransaction();
        string TokenCreate();

        ResponseMessage GetPayfabricTransactionLog(string documentAmount, string shipToLine1);
    }
}
