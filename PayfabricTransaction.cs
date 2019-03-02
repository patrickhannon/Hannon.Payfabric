using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Runtime.Serialization;
using System.Text;
using RestSharp;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using Hannon.Util;
using Hannon.PayFabric.Models;
using Hannon.Utils;


namespace Hannon.PayFabric
{
    public class PayfabricTransaction : ITransaction
    {
        #region Private vars
        private readonly Dictionary<Command, TransactionCommand> _payFabricCommands = new Dictionary<Command, TransactionCommand>();
        private readonly string _auth;
        private JavaScriptSerializer _serializer;
        private readonly string _hostAppName = "PayfabricTransaction";
        #endregion

        #region Constructors

        public PayfabricTransaction(string deviceId, string devicePwd, string url)
        {
            ArgumentValidator.ThrowOnNullEmptyOrWhitespace("deviceId", deviceId);
            ArgumentValidator.ThrowOnNullEmptyOrWhitespace("devicePwd", devicePwd);
            ArgumentValidator.ThrowOnNullEmptyOrWhitespace("url", url);
            _auth = string.Format("{0}|{1}", deviceId, devicePwd);
            //System.Net.ServicePointManager.SecurityProtocol |= SecurityProtocolType.Tls11 | SecurityProtocolType.Tls12;
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            _serializer = new JavaScriptSerializer();
          
            _payFabricCommands.Add(Command.CreateToken, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/token/create", url),
                Name = "CreateToken",
            });
            _payFabricCommands.Add(Command.CreateTrans, new TransactionCommand()
            {
                HttpVerb = Method.POST,
                Url = string.Format("{0}/payment/api/transaction/create", url),
                Name = "CreateTrans",
            });
            _payFabricCommands.Add(Command.ProcessTrans, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/transaction/process/{{key}}", url, string.Empty),
                Name = "ProcessTrans",
            });
            _payFabricCommands.Add(Command.GetCardsByCustomer, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/wallet/get/{{customer}}?tender=CreditCard", url),
                Name = "GetCardsByCustomer",
            });
            _payFabricCommands.Add(Command.RetrieveTrans, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/transaction/{1}", url, string.Empty),
                Name = "RetrieveTrans",
            });
            _payFabricCommands.Add(Command.PreAuthTrans, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/reference/{{key}}?trxtype=Ship", url, string.Empty),
                Name = "PreAuthTrans",
            });
            _payFabricCommands.Add(Command.CreateWallet, new TransactionCommand()
            {
                HttpVerb = Method.POST,
                Url = string.Format("{0}/payment/api/wallet/create", url),
                Name = "CreateWallet",
            });
            _payFabricCommands.Add(Command.UpdateWallet, new TransactionCommand()
            {
                HttpVerb = Method.POST,
                Url = string.Format("{0}/payment/api/wallet/update", url),
                Name = "UpdateWallet",
            });
            _payFabricCommands.Add(Command.DeleteWallet, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/wallet/delete/", url),
                Name = "DeleteWallet",
            });
            _payFabricCommands.Add(Command.GetCardByCustomerWallet, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/wallet/get/{{walletId}}", url),
                Name = "GetCardByCustomerWallet",
            });
            _payFabricCommands.Add(Command.CancelTrans, new TransactionCommand()
            {
                HttpVerb = Method.GET,
                Url = string.Format("{0}/payment/api/reference/{{trx_key}}?trxtype=VOID", url),
                Name = "CancelTrans",
            });
        }
        
        #endregion

        #region Public methods

        public ResponseModel CancelTrans(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            throw new NotImplementedException();
        }

        public ResponseModel CancelTransaction(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            bool success = false;
            TransactionCommand command;
            var responseValue = new PayFabricResponseModel();
            _payFabricCommands.TryGetValue(Command.CancelTrans, out command);
            var url = command.Url.Replace("{trx_key}", model.Key);
            var client = new RestClient(url);
            var request = new RestRequest(command.HttpVerb);
            request.AddHeader("authorization", _auth);
            request.AddHeader("content-type", "application/json");
            var encodedTrans = Json.Encode(model);
            request.AddParameter("application/json", encodedTrans, ParameterType.RequestBody);
            var response = client.Execute(request);
            var keyResponse = response.Content;
            dynamic decodedResult = Json.Decode(keyResponse);
            var authKey = decodedResult.Key;

            if (Helpers.ResponseStatus(response.StatusCode))
            {
                success = true;
                responseValue = GetResult(keyResponse);
            }
            else
            {
                success = false;
            }
            return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(), new PayfabricTransactionResponse());

        }

        public ResponseModel CreatePreAuthTrans(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            TransactionCommand command;
            bool success = false;
            var responseValue = new PayFabricResponseModel();
            _payFabricCommands.TryGetValue(Command.PreAuthTrans, out command);
            var url = command.Url.Replace("{key}", model.Key);
            var client = new RestClient(url);
            var request = new RestRequest(command.HttpVerb);
            request.AddHeader("authorization", _auth);

            request.AddHeader("content-type", "application/json");
            var encodedTrans = Json.Encode(model);
            request.AddParameter("application/json", encodedTrans, ParameterType.RequestBody);
            var response = client.Execute(request);
            var keyResponse = response.Content;
            if (Helpers.ResponseStatus(response.StatusCode))
            {
                success = true;
                responseValue = GetResult(keyResponse);
            }
            else
            {
                success = false;
            }
            return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(),
                new PayfabricTransactionResponse());

        }

        public PayFabricResponseModel CreateSecurityToken()
        {
            var responseValue = new PayFabricResponseModel();
            {
                var success = false;
                TransactionCommand command;
                _payFabricCommands.TryGetValue(Command.CreateToken, out command);
                var client = new RestClient(command.Url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                var response = client.Execute(request);
                var keyResponse = response.Content;
                if (Helpers.ResponseStatus(response.StatusCode))
                {
                    success = true;
                    responseValue = GetResult(keyResponse);
                }
                else
                {
                    success = false;
                }
                return responseValue;
            }
        }

        public ResponseModel CreateSecurityToken(string deviceId, string payfabricGatewayAccountProfile,
            string payfabricUrl)
        {
            //
            var responseValue = new PayFabricResponseModel();
            {
                var success = false;
                TransactionCommand command;
                _payFabricCommands.TryGetValue(Command.CreateToken, out command);
                var client = new RestClient(command.Url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                var response = client.Execute(request);
                var keyResponse = response.Content;
                if (Helpers.ResponseStatus(response.StatusCode))
                {
                    success = true;
                    responseValue = GetResult(keyResponse);
                }
                else
                {
                    success = false;
                }
                return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(),
                    new PayfabricTransactionResponse());
            }
        }

        public ResponseModel CreateTrans(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            bool success = false;
            TransactionCommand command;
            var responseValue = new PayFabricResponseModel();
            IRestResponse response = new RestResponse();
            var keyResponse = string.Empty;
            try
            {
                //_logger.Log(LogLevel.Info, "Processing Transaction | " + model.ToString());
                _payFabricCommands.TryGetValue(Command.CreateTrans, out command);
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var client = new RestClient(command.Url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                request.AddHeader("content-type", "application/json");
                var encodedTrans = Json.Encode(model);
                request.AddParameter("application/json", encodedTrans, ParameterType.RequestBody);
                response = client.Execute(request);
                keyResponse = response.Content;
                dynamic decodedResult = Json.Decode(keyResponse);
                var authKey = decodedResult.Key;
                //_logger.Log(LogLevel.Info, response.Content);
            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, response.Content);
                //_logger.Log(LogLevel.Error, ex, ex.StackTrace);
                throw new Exception(response.Content, ex);
            }
            if (Helpers.ResponseStatus(response.StatusCode))
            {
                success = true;
                responseValue = GetResult(keyResponse);
            }
            else
            {
                success = false;
            }
            return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(),
                new PayfabricTransactionResponse());

        }

        public ResponseModel CreateWallet(PayfabricWalletCreateModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            var success = false;
            var responseValue = new PayFabricResponseModel();
            IRestResponse response = new RestResponse();
            TransactionCommand command;
            try
            {
                _payFabricCommands.TryGetValue(Command.CreateWallet, out command);
                var client = new RestClient(command.Url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                request.AddHeader("content-type", "application/json");
                var json = _serializer.Serialize(model);
                //var json = ConvertToTransactionalModelAsString();
                request.AddParameter("application/json", json, ParameterType.RequestBody);
                response = client.Execute(request);
                var keyResponse = response.Content;

                if (Helpers.ResponseStatus(response.StatusCode))
                {
                    success = true;
                    responseValue = GetResult(keyResponse);
                }
                else
                {
                    success = false;
                }
                //_logger.Log(LogLevel.Info, keyResponse);

                return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(), new PayfabricTransactionResponse());

            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, ex, ex.StackTrace);
                throw (ex);
            }
        }

        public ResponseModel DeleteWallet(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            TransactionCommand command;
            var responseValue = new PayFabricResponseModel();
            var success = false;
            var walletId = Guid.Empty;
            _payFabricCommands.TryGetValue(Command.DeleteWallet, out command);
            var client = new RestClient(command.Url + model.Value);
            var request = new RestRequest(command.HttpVerb);
            request.AddHeader("authorization", _auth);
            request.AddHeader("content-type", "application/json");
            var json = _serializer.Serialize(model);
            var response = client.Execute(request);
            var keyResponse = response.Content;

            if (Helpers.ResponseStatus(response.StatusCode))
            {
                success = true;
                if (keyResponse != null)
                {
                    responseValue = GetResult(keyResponse);
                }
            }
            else
            {
                success = false;
            }
            //walletId
            return ConvertToResponseModel(response, success, new PayFabricResponseModel() { Result = walletId.ToString() },
                new List<PayfabricCustomerResponseModel>(), new PayfabricTransactionResponse());
        }

        public ResponseModel GetCardByCustomerWallet(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            var success = false;
            var walletId = Guid.Empty;
            var message = string.Empty;
            var key = string.Empty;
            ArgumentValidator.ThrowOnNull("model", model);
            TransactionCommand command;
            List<PayfabricCustomerResponseModel> customerPaymentDetails = new List<PayfabricCustomerResponseModel>();
            _payFabricCommands.TryGetValue(Command.GetCardByCustomerWallet, out command);
            var url = command.Url.Replace("{walletId}", model.Token);
            var client = new RestClient(url);
            var request = new RestRequest(command.HttpVerb);
            request.AddHeader("authorization", _auth);
            var response = client.Execute(request);
            var keyResponse = response.Content;

            //"[{"Aba":"","Account":"XXXXXXXXXXX1007","AccountType":"","Billto":{"City":"","Country":"","Customer":"","Email":"","ID":"00000000-0000-0000-0000-000000000000","Line1":"","Line2":"","Line3":"","ModifiedOn":"1/1/0001 12:00:00 AM","Phone":"","State":"","Zip":""},"CAUClosedFlag":"","CardHolder":{"DriverLicense":"","FirstName":"Patrick","LastName":"Hannon","MiddleName":"","SSN":""},"CardName":"AmericanExpress","CheckNumber":null,"Connector":"","Customer":"6","ExpDate":"0121","GPAddressCode":"","GatewayToken":"","ID":"79f26f72-22e4-491f-9642-0036fe718c78","Identifier":"","IsDefaultCard":false,"IsLocked":false,"IsSaveCard":false,"IssueNumber":"","ModifiedOn":"11/11/2016 9:07:59 AM","StartDate":"","Tender":"CreditCard","UserDefine1":"","UserDefine2":"","UserDefine3":"","UserDefine4":""}]"
            if (Helpers.ResponseStatus(response.StatusCode) && !string.IsNullOrEmpty(response.Content))
            {
                customerPaymentDetails.Add(_serializer.Deserialize<PayfabricCustomerResponseModel>(response.Content));
                success = true;
                message = "Wallets were found for that account";
            }
            else
            {
                success = false;
                message = "No wallets were found for that account";
            }
            return ConvertToResponseModel(response, success, new PayFabricResponseModel()
            {
                Result = walletId.ToString(),
                Message = message,
                Key = string.Empty
            },
            customerPaymentDetails, new PayfabricTransactionResponse());
        }

        public ResponseModel GetCardsByCustomer(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            List<PayfabricCustomerResponseModel> customerPaymentDetails = new List<PayfabricCustomerResponseModel>();
            IRestResponse response;
            var success = false;
            var walletId = Guid.Empty;
            var message = string.Empty;
            try
            {
                TransactionCommand command;
                _payFabricCommands.TryGetValue(Command.GetCardsByCustomer, out command);
                var url = command.Url.Replace("{customer}", model.Customer);
                var client = new RestClient(url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                response = client.Execute(request);
                var keyResponse = response.Content;
                if (Helpers.ResponseStatus(response.StatusCode) && !string.IsNullOrEmpty(response.Content))
                {
                    customerPaymentDetails = _serializer.Deserialize<List<PayfabricCustomerResponseModel>>(response.Content);
                    success = true;
                    message = "Wallets were found for that account";
                }
                else
                {
                    success = false;
                    message = "No wallets were found for that account";
                }
                //_logger.Log(LogLevel.Info, response.Content);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //_logger.Log(LogLevel.Error, ex, ex.StackTrace);
                throw;
            }
            return ConvertToResponseModel(response, success, new PayFabricResponseModel() { Result = walletId.ToString(), Message = message },
                customerPaymentDetails, new PayfabricTransactionResponse());
        }

        public ResponseModel ProcessTransaction(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            TransactionCommand command;
            bool success = false;
            var responseValue = new PayFabricResponseModel();
            var payfabricTransactionResponse = new PayfabricTransactionResponse();
            IRestResponse response = new RestResponse();
            var keyResponse = string.Empty;
            try
            {
                _payFabricCommands.TryGetValue(Command.ProcessTrans, out command);
                var url = command.Url.Replace("{key}", model.Key);
                var client = new RestClient(url);
                var request = new RestRequest(command.HttpVerb);
                //request.AddHeader("authorization", _auth);
                var token = CreateSecurityToken();
                request.AddHeader("authorization", token.Token);
                request.AddHeader("content-type", "application/json");
                response = client.Execute(request);
                keyResponse = response.Content;
                if (Helpers.ResponseStatus(response.StatusCode))
                {
                    success = true;
                    responseValue = GetResult(keyResponse);
                    payfabricTransactionResponse = _serializer.Deserialize<PayfabricTransactionResponse>(response.Content);
                    //write the response to the logs, for debugging
                    //_logger.Log(LogLevel.Info, response.Content);
                }
                else
                {
                    success = false;
                }
                return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(), payfabricTransactionResponse);
            }
            catch (Exception ex)
            {
                //_logger.Log(LogLevel.Error, ex, response.Content);
                //_logger.Log(LogLevel.Error, ex, ex.StackTrace);
                throw new Exception(response.Content, ex);
            }
        }
        
        public ResponseModel RetrieveTransaction(TransactionModel model)
        {
            ArgumentValidator.ThrowOnNull("model", model);
            TransactionCommand command;
            bool success = false;
            var responseValue = new PayFabricResponseModel();
            _payFabricCommands.TryGetValue(Command.RetrieveTrans, out command);

            //Replace with actual key for the transaction
            var url = string.Format(command.Url, model.Key);
            var client = new RestClient(url);
            var request = new RestRequest(command.HttpVerb);
            request.AddHeader("authorization", _auth);
            IRestResponse response = client.Execute(request);
            var keyResponse = response.Content;
            if (Helpers.ResponseStatus(response.StatusCode))
            {
                success = true;
                responseValue = GetResult(keyResponse);
            }
            else
            {
                success = false;
            }
            return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(),
                new PayfabricTransactionResponse());
        }

        public void TestTransaction()
        {
            try
            {
                //  Populate POST String
                StringBuilder datastring = new StringBuilder();
                datastring.Append("{");
                datastring.Append("\"Customer\":\"326890\",");
                datastring.Append("\"Currency\":\"USD\",");
                datastring.Append("\"Amount\":\"4.88\",");
                datastring.Append("\"Type\":\"Sale\",");
                datastring.Append("\"SetupId\":\"MarksTester\""); // Replace with your gateway account profile name
                datastring.Append("}");

                // POST
                byte[] data = System.Text.Encoding.UTF8.GetBytes(datastring.ToString());
                var url = "https://sandbox.payfabric.com/V2/Rest/api/transaction/create";
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Headers["authorization"] = _auth;
                //httpWebRequest.Headers["authorization"] = new Token().Create();
                httpWebRequest.Method = "POST";
                httpWebRequest.ContentLength = data.Length;
                Stream stream = httpWebRequest.GetRequestStream();
                stream.Write(data, 0, data.Length);
                stream.Close();
                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();

                // Parse response
                TrxKeyResponse obj = JsonHelper.JsonDeserialize<TrxKeyResponse>(result);
                var key = obj.Key;

                ProcessTestTransaction(key);

            }
            catch (WebException)
            {
                //  Handling exception from PayFabric
            }
            catch (Exception)
            {
                //  Handling exception
            }
        }

        public void TestTokenCreate()
        {
            try
            {
                // Replace url when going live
                //https://www.payfabric.com/
                //var url = "https://www.payfabric.com/payment/api/token/create";
                //var url = "https://sandbox.payfabric.com/payment/api/token/create";
                var url = "https://sandbox.payfabric.com/Payment/Web";
                //var url = "https://sandbox.payfabric.com/payment/api/token/create";
                //https://sandbox.payfabric.com/V3/PayFabric/rest/payment/api/token/create
                //var url = "https://sandbox.payfabric.com/payment/api/token/create";
                HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
                httpWebRequest.ContentType = "application/json; charset=utf-8";
                httpWebRequest.Method = "GET";

                // Replace with your own device id and device password
                httpWebRequest.Headers["authorization"] = _auth;

                HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                Stream responseStream = httpWebResponse.GetResponseStream();
                StreamReader streamReader = new StreamReader(responseStream);
                string result = streamReader.ReadToEnd();
                streamReader.Close();
                responseStream.Close();
                httpWebRequest.Abort();
                httpWebResponse.Close();

                // Parse JSON response
                //TokenResponse obj = JsonHelper.JsonDeserialize<TokenResponse>(result);
                //return obj.Token;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public ResponseMessage GetPayfabricTransactionLog(string documentAmount, string shipToLine1)
        {
            ArgumentValidator.ThrowOnNullEmptyOrWhitespace("documentAmount", documentAmount);
            ArgumentValidator.ThrowOnNullEmptyOrWhitespace("shipToLine1", shipToLine1);
            //var logs =_payfabricWalletRepository.GetPayfabricTransactionLog(documentAmount, shipToLine1);

            //Check in log for response codes, then return that specific response code back in response

            return new ResponseMessage();
        }

        public ResponseModel UpdateWallet(PayfabricWalletRequestModel model)
        {
            //PayFabric cannot update the card number of an existing card. To update a card number, 
            //delete the old card record and create a new one.
            ArgumentValidator.ThrowOnNull("model", model);
            var success = false;
            var responseValue = new PayFabricResponseModel();
            List<PayfabricCustomerResponseModel> customerPaymentDetails = new List<PayfabricCustomerResponseModel>();
            IRestResponse response = new RestResponse();
            TransactionCommand command;
            var message = string.Empty;
            try
            {
                _payFabricCommands.TryGetValue(Command.UpdateWallet, out command);
                var client = new RestClient(command.Url);
                var request = new RestRequest(command.HttpVerb);
                request.AddHeader("authorization", _auth);
                request.AddHeader("content-type", "application/json");
                var encodedTrans = Json.Encode(model);
                request.AddParameter("application/json", encodedTrans, ParameterType.RequestBody);
                //https://www.payfabric.com/Payment/Web//profile/WalletDetails?id=d46a5b27-06a9-480b-b79b-ff126c575520&ServiceInstanceId=1f2ccbe2-dc9e-497b-81d0-f11efbf79bc5&InstFlag=1&NavFlag=0&DCN=1
                response = client.Execute(request);
                var keyResponse = response.Content;
                if (Helpers.ResponseStatus(response.StatusCode) && !string.IsNullOrEmpty(response.Content))
                {
                    customerPaymentDetails = _serializer.Deserialize<List<PayfabricCustomerResponseModel>>(response.Content);
                    success = true;
                    responseValue.Message = "Wallet was saved with the updated billing information";
                }
                else
                {
                    success = false;
                    responseValue.Message = "Wallet was not saved with the updated billing information";
                }
                //_logger.Log(LogLevel.Info, response.Content);
                return ConvertToResponseModel(response, success, responseValue, new List<PayfabricCustomerResponseModel>(), new PayfabricTransactionResponse());  
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                //_logger.Log(LogLevel.Error, ex, ex.StackTrace);
                throw (ex);
            }
        }
        
        #endregion

        #region Private methods

        private PayFabricResponseModel GetResult(string resResult)
        {
            return _serializer.Deserialize<PayFabricResponseModel>(resResult);
        }

        private void ProcessTestTransaction(string key)
        {
            var url = "https://sandbox.payfabric.com/V2/Rest/api/transaction/process/" + key;
            HttpWebRequest httpWebRequest = WebRequest.Create(url) as HttpWebRequest;
            httpWebRequest.Method = "GET";
            httpWebRequest.ContentType = "application/json; charset=utf-8";
            httpWebRequest.Headers["authorization"] = _auth;
            //httpWebRequest.Headers["authorization"] = new Token().Create();
            HttpWebResponse httpWebResponse = httpWebRequest.GetResponse() as HttpWebResponse;
            Stream responseStream = httpWebResponse.GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream);
            string result = streamReader.ReadToEnd();
            Debug.WriteLine(result);
            streamReader.Close();
            responseStream.Close();
            httpWebRequest.Abort();
            httpWebResponse.Close();

        }

        private static string ConvertToTransactionalModelAsString()
        {
            string s = @" {
              'Tender': 'CreditCard',
              'Customer': ' 1013277',
              'Account': ' 4111111111111111',
              'ExpDate': '1222',
              'CardHolder': {
                'FirstName': 'Patrick',
                'LastName': 'Hannon'
              },
              'Billto': {
                'Customer': 'Patrick Hannon ',
                'Line1': '7307 Heritage Oaks Court',
                'City': 'Arlington',
                'State': 'TX',
                'Country': 'US',
                'Zip': '76001'
              }
           }";
            return s;
        }
        
        private ResponseModel ConvertToResponseModel(IRestResponse response, bool success, PayFabricResponseModel responseValue, List<PayfabricCustomerResponseModel> customerPaymentDetails,  PayfabricTransactionResponse transactionalResponse)
        {
            return new ResponseModel()
            {
                Success = success,
                Message = response.Content,
                ResponseValue = responseValue,
                CustomerPaymentDetails = customerPaymentDetails,
                TransactionResponse = transactionalResponse
            };
        }
        
        #endregion

        #region Sub-classes

        [DataContract]
        public class TrxKeyResponse
        {
            [DataMember]
            public string Key { get; set; }
        }

        #endregion
    }
}
