using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CowFarmApp2.Model {
    public class VeterinarData : Event {
        //public int Id { get; set; }
        public string Govedo_Id { get; set; }
        public string Diagnoza { get; set; }
        public string Liječenje { get; set; }
        public string Datum_Pregleda { get; set; }

        public VeterinarData() {
            Naslov = "Veterinar";
        }
    }

    public class VeterinarDataCreate {
        public string Id { get; set; }
        public string Govedo_Id { get; set; }
        public string Diagnoza { get; set; }
        public string Liječenje { get; set; }
        public string Datum { get; set; }
    }

    public class VeterinarDataObjects {
        public ObservableCollection<VeterinarData> Data { get; set; }
    }
}
