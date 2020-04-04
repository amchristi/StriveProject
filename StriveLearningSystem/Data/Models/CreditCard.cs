using System;
using System.Collections.Generic;
using System.Text;

namespace Data.Models
{
    public class CreditCard
    {
        public string CardHolderName { get; set; }
        public string CardNumber { get; set; }
        public string ExpMonth{ get; set; }
        public string ExpYear { get; set; }
        public string cvc { get; set; }

    }
}
