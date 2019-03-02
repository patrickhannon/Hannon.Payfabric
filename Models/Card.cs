using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class Card
    {
        public string Account { get; set; }
        public string ExpDate { get; set; }
        public CardHolder CardHolder { get; set; }
        public string ID { get; set; }

        public override string ToString()
        {
            return "ID:" + ID + ",Account:" + Account + ",ExpDate:" + ExpDate;
        }
    }
}
