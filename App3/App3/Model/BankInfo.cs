using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class BankInfo
    {
        public int ID { get; set; }
        public string DU { get; set; }
        public string ShortName { get; set; }
        public string Bic { get; set; }
        public string RegNum { get; set; }
        public string RegNum_date { get; set; }
        public int IntCode { get; set; }
        public int RegNumber { get; set; }

        public static implicit operator BankInfo(List<BankInfo> v)
        {
            throw new NotImplementedException();
        }



    }
}
