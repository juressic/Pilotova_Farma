using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using SQLite;

namespace CowFarmApp2.Model {
    public class WeightData : Event {

        //[PrimaryKey]
        //public int Id { get; set; }
        public string Govedo_Id { get; set; }
        public string Izmjerena_Težina { get; set; }
        public string Datum_Pregleda { get; set; }

        public WeightData() {
            Naslov = "Vaganje";
        }

    }
    public class WeightDataObjects {
        public ObservableCollection<WeightData> Data { get; set; }
    }
}
