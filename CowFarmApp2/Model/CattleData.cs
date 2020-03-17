using System;
using System.Collections.Generic;
using System.Text;
using SQLite;
namespace CowFarmApp2.Model
{
    public class CattleData
    {
        public int Id { get; set; }
        public string Broj_Životinje { get; set; }
        public string Redni_Broj { get; set; }
        public string Remen { get; set; }
        public string IKG_Broj { get; set; }
        public string JIBG { get; set; }
        public string Spol { get; set; }
        public string Rasa { get; set; }
        public string Uzrast { get; set; }
        public string Datum_Rođenja { get; set; }
        public string Majka { get; set; }
        public string Otac { get; set; }
        public string Farma { get; set; }
        public string Mjesto { get; set; }
        public string Boks { get; set; }
        public string Vlasnik { get; set; }
        public string Težina { get; set; }
        public string Aktivnost { get; set; }
        public string Stari_Vlasnik { get; set; }
        public string Datum_Dolaska { get; set; }
        public string Mjesto_Odlaska { get; set; }
        public string Datum_Odlaska { get; set; }
        public string Način_Odlaska { get; set; }
        public string Bolestan { get; set; }
        public string Oznaka_Uha { get; set; }
        public string Gravidnost { get; set; }

    }
}
