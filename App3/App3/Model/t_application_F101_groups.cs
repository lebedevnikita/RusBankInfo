using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace App3.Model
{
    public class t_application_F101_groups 
    {
        public string dt { get; set; }
        public string pln { get; set; }
        public string IndCode { get; set; }
        public Nullable<long> ap { get; set; }
        public string regn { get; set; }
        public Nullable<long> col_1 { get; set; }
        public Nullable<long> col_2 { get; set; }
        public Nullable<long> col_3 { get; set; }

        public static implicit operator t_application_F101_groups(List<t_application_F101_groups> v)
        {
            throw new NotImplementedException();
        }

    }
}
