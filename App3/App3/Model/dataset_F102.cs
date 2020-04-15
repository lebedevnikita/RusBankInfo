using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class Dataset_F102
    {
        public string regn { get; set; }
        public Nullable<System.DateTime> dt { get; set; }
        public string FieldName { get; set; }
        public Nullable<int> Field_id { get; set; }
        public Nullable<long> val { get; set; }

        public static implicit operator Dataset_F102(List<Dataset_F102> v)
        {
            throw new NotImplementedException();
        }
    }
}
