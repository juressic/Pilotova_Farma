using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace CowFarmApp2.Model {
    public class StatusData {

        public int Id { get; set; }
        public int Govedo_Id { get; set; }
        public string Bolestan { get; set; }
        public string Gravidnost { get; set; }
        public string Oznaka_Uha { get; set; }
    }

    public class StatusDataObjects {
        public ObservableCollection<StatusData> Data { get; set; }
    }
}
