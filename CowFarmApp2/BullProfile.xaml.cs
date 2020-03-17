using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SQLite;
using CowFarmApp2.Model;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BullProfile : ContentPage
    {
        public int IdNumber;

        public BullProfile(int ID)
        {
            InitializeComponent();
            IdNumber = ID;
        }

        public int Remen { get; set; }

        [MaxLength(50)]
        public string BrojŽivotinje { get; set; }

        public int SkraćeniBroj { get; set; }

        public string Spol { get; set; }

        public string Rasa { get; set; }

        public string Uzrast { get; set; }
        
        public string Lokacija { get; set; }

        public string DatumRođenja { get; set; }

        public string Migracija_KPUG { get; set; }

        public string Majka { get; set; }

        public string Otac { get; set; }

        public string Isporuka { get; set; }

        public string Napomena { get; set; }

        public string DatumDolaska { get; set; }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            //DisplayAlert("Number Found", "Item Number = " + BrojŽivotinje, "OK");
            Broj_Životinje.Text = BrojŽivotinje;

        }
    }
}