using System;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class AddressModel
    {
        public string Customer { get; set; }
        public string Line1 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public string Zip { get; set; }
        public string Email { get; set; }
        public override string ToString()
        {
            return string.Format("Customer:{0},Line1:{1},City:{2},State:{3},Country:{4},Zip:{5},Email:{6}",
                Customer, Line1, City, State, Country, Zip, Email);
        }
    }
}
