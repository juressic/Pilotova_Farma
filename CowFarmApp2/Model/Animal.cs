using System;
using System.Collections.Generic;
using System.Text;

namespace CowFarmApp2.Model {
    public class Animal {

        public GravidnostData gravidnostData;

        public string datumZačeća { get; set; }

        public string datumTeljenja { get; set; }

        public int remaningDays { get; set; }

        public string icon { get; set; }

        public List<Cattle> potomci = new List<Cattle>();
    }
}
