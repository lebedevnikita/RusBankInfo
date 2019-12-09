using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class t_dates
    {
        public string obj { get; set; }
        public string dt { get; set; }
        public string dt_mmmmYYYY { get; set; }

        public static implicit operator t_dates(List<t_dates> v)
        {
            throw new NotImplementedException();
        }
    }
}
