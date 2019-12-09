using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class IndCodekInfo
    {
        public int IndID { get; set; }
        public string IndCode { get; set; }
        public string Name { get; set; }
        public int IndType { get; set; }
        public string IndChapter { get; set; }
        // public DateTime LOAD_DT { get; set; }

        public static implicit operator IndCodekInfo(List<IndCodekInfo> v)
        {
            throw new NotImplementedException();
        }


    }
}
