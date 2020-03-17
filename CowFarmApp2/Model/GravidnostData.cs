using SQLite;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CowFarmApp2.Model {
    public class GravidnostData : Event {
        
        //public int Id { get; set; }
        public int Govedo_Id { get; set; }
        public string Datum_Začeća { get; set; }
        public string Datum_Pregleda { get; set; }
        //public string Datum { get { return Datum_Pregleda; } }

        public GravidnostData() {
            Naslov = "Gravidnost";
        }
    }

    class GravidnostDataObjects {
        public ObservableCollection<GravidnostData> Data { get; set; }
    }
}
