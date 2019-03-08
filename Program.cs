using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Hannon.PayFabric
{
    public class Program
    {
        private ITransaction _transaction;
        private static string _payfabricDeviceId;
        private static string _payfabricDevicePassword;
        private static string _payfabricDeviceUrl;
        static void Main(string[] args)
        {
            _payfabricDeviceId = ConfigurationManager.AppSettings["PayfabricDeviceId"];
            _payfabricDevicePassword = ConfigurationManager.AppSettings["PayfabricDevicePassword"];
            _payfabricDeviceUrl = ConfigurationManager.AppSettings["PayfabricDeviceUrl"];

            Program p = new Program();
            p.TestPayfabricTokenCreate();
        }
        private void TestPayfabricTokenCreate()
        {
            _transaction = new PayfabricTransaction(_payfabricDeviceId,
                _payfabricDevicePassword, _payfabricDeviceUrl);
            var response = _transaction.CreateSecurityToken();

            Debug.WriteLine(response.StatusCode);
        }
    }
}
