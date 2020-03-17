using System;
using System.Collections.Generic;
using System.Text;
using SQLite;

namespace CowFarmApp2.Model {
    public class Event {

        [PrimaryKey]
        public int Id { get; set; }
        public string Naslov { get; set; }
        public string Datum { get; set; }
        public string Icon { get; set; }
    }
}
