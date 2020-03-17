using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CowFarmApp2.Model {
    public class AbortusData : Event {
        //[PrimaryKey]
        //public int Id { get; set; }
        public int Govedo_Id { get; set; }
        public string Datum_Abortusa { get; set; }

        public AbortusData() {
            Naslov = "Abortus";
            Icon = "icons8_abortion_100.png";
        }

    }
    public class AbortusDataObjects {
        public ObservableCollection<AbortusData> Data { get; set; }
    }

}
