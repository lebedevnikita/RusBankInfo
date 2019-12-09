using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class F101_actives_top_n
    {
        public int Id { get; set; }
        public string dt { get; set; }
        public string ShortName { get; set; }
        public float Val { get; set; }
        
        public int n { get; set; }
      

        public static implicit operator F101_actives_top_n(List<F101_actives_top_n> v)
        {
            throw new NotImplementedException();
        }
    }
}
