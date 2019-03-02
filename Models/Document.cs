using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hannon.PayFabric.Models
{
    [Serializable]
    public class Document
    {
        public List<Head> Head { get; set; }
    }
}
