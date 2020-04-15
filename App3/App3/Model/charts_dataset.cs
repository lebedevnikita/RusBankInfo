using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
  

    public class Charts_dataset
    {
        public string dt { get; set; }
        public Nullable<long> val { get; set; }

        public static implicit operator Charts_dataset(List<Charts_dataset> v)
        {
            throw new NotImplementedException();
        }
    }
}
