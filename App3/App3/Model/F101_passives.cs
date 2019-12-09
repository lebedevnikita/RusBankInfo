using System;
using System.Collections.Generic;
using System.Text;

namespace App3.Model
{
    public class F101_passives

    {
        public int Id { get; set; }
        public string Label { get; set; }
        public float Val { get; set; }

        public static implicit operator F101_passives(List<F101_passives> v)
        {
            throw new NotImplementedException();
        }
    }
}
