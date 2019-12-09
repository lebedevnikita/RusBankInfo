using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class F101_actives
    {
            public int Id { get; set; }
            public string Label { get; set; }
            public float Val { get; set; }


        public static implicit operator F101_actives(List<F101_actives> v)
        {
            throw new NotImplementedException();
        }


    }
}
