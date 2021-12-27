using System;
using System.Collections.Generic;
using System.Text;

namespace Lab4_sa
{
    class CurrencyRoot
    {
        public string table { get; set; }
        public string currency { get; set; }
        public string code { get; set; }
        public List<CurrencyRate> rates { get; set; }
    }
}
