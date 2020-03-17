using CowFarmApp2.Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateCattle : ContentPage
    {
        public UpdateCattle()
        {
            InitializeComponent();

            GenderPicker.Items.Add("Ž");
            GenderPicker.Items.Add("M");

            RacePicker.Items.Add("ANGUS");
            RacePicker.Items.Add("ŠAROLE");
            RacePicker.Items.Add("SIMENTALAC");
            RacePicker.Items.Add("LIMUZIN");
            RacePicker.Items.Add("PIEMONT");
            RacePicker.Items.Add("LIM-ANG");
            RacePicker.Items.Add("LIM-ŠAR");
            RacePicker.Items.Add("LIM-SIM");
            RacePicker.Items.Add("LIM-PIE");
            RacePicker.Items.Add("SIM-ANG");
            RacePicker.Items.Add("SIM-ŠAR");
            RacePicker.Items.Add("SIM-PIE");
            RacePicker.Items.Add("PIE-ANG");
            RacePicker.Items.Add("PIE-ŠAR");
            RacePicker.Items.Add("ŠAR-SIM");
            RacePicker.Items.Add("ŠAR-ANG");

            UzrastPicker.Items.Add("Krava");
            UzrastPicker.Items.Add("Tele");
            UzrastPicker.Items.Add("Junac");
            UzrastPicker.Items.Add("Junica");
            UzrastPicker.Items.Add("Bik");

            NačinOdlaskaPicker.Items.Add("PRODAJA");
            NačinOdlaskaPicker.Items.Add("KLANJE");
            NačinOdlaskaPicker.Items.Add("UGINUĆE");
            NačinOdlaskaPicker.Items.Add("KRAĐA-GUBITAK");

            AktivnostPicker.Items.Add("DA");
            AktivnostPicker.Items.Add("NE");

            PopulateFields();

        }


        private void PopulateFields()
        {
            BrojŽivotinjeEntry.Text = App.currentCattle.Broj_Zivotinje;
            RedniBrojEntry.Text = App.currentCattle.Redni_Broj;
            RemenEntry.Text = App.currentCattle.Remen;
            IKGBrojEntry.Text = App.currentCattle.IKG_Broj;
            JibgEntry.Text = App.currentCattle.JIBG;
            GenderPicker.SelectedItem = App.currentCattle.Spol;
            RacePicker.SelectedItem = App.currentCattle.Rasa;
            UzrastPicker.SelectedItem = App.currentCattle.Uzrast;
            //RođenjeDatePicker.Date = DateTime.Parse(App.currentCattle.Datum_Rodenja);
            //Datum_Rođenja_DateLabel.Text = App.currentCattle.Datum_Rodenja == "0000-00-00" ? "" : Datum_Dolaska_DateLabel.BindingContext = RođenjeDatePicker;
            if(App.currentCattle.Datum_Rodenja != "0000-00-00")
            {
                RođenjeDatePicker.Date = DateTime.Parse(App.currentCattle.Datum_Rodenja);
                Datum_Rođenja_DateLabel.BindingContext = RođenjeDatePicker;
            }
            //Datum_Rođenja_DateLabel.Text = "test";
            MajkaEntry.Text = App.currentCattle.Majka;
            OtacEntry.Text = App.currentCattle.Otac;
            FarmaEntry.Text = App.currentCattle.Farma;
            MjestoEntry.Text = App.currentCattle.Mjesto;
            BoxEntry.Text = App.currentCattle.Boks;
            TezinaEntry.Text = App.currentCattle.Tezina;
            VlasnikEntry.Text = App.currentCattle.Vlasnik;
            AktivnostPicker.SelectedItem = App.currentCattle.Aktivnost;
            StariVlasnikEntry.Text = App.currentCattle.Stari_Vlasnik;
            if (App.currentCattle.Datum_Dolaska != "0000-00-00")
            {
                DolazakDatePicker.Date = DateTime.Parse(App.currentCattle.Datum_Dolaska);
                Datum_Dolaska_DateLabel.BindingContext = DolazakDatePicker;
            }
            MjestoOdlaskaEntry.Text = App.currentCattle.Mjesto_Odlaska;
            if (App.currentCattle.Datum_Odlaska != "0000-00-00")
            {
                OdlazakDatePicker.Date = DateTime.Parse(App.currentCattle.Datum_Odlaska);
                Datum_Odlaska_DateLabel.BindingContext = OdlazakDatePicker;
            }
            NačinOdlaskaPicker.SelectedItem = App.currentCattle.Nacin_Odlaska;
            //DolazakDatePicker.Date = DateTime.Parse(App.currentCattle.Datum_Dolaska);
            //Datum_Dolaska_DateLabel.Text = App.currentCattle.Datum_Dolaska == "0000-00-00" ? "" : App.currentCattle.Datum_Dolaska;
            //DolazakDatePicker.Date = 

        }

        private async void Update_Clicked(object sender, EventArgs args)
        {
            try
            {
                CattleData cattle = new CattleData() {
                    Broj_Životinje = BrojŽivotinjeEntry.Text,
                    Redni_Broj = RedniBrojEntry.Text,
                    Remen = RemenEntry.Text,
                    IKG_Broj = IKGBrojEntry.Text,
                    JIBG = JibgEntry.Text,
                    Spol = (string)GenderPicker.SelectedItem,
                    Rasa = (string)RacePicker.SelectedItem,
                    Uzrast = (string)UzrastPicker.SelectedItem,
                    Datum_Rođenja = Datum_Rođenja_DateLabel.Text != null ? Datum_Rođenja_DateLabel.Text : "0000-00-00",
                    Majka = MajkaEntry.Text,
                    Otac = OtacEntry.Text,
                    Farma = FarmaEntry.Text,
                    Mjesto = MjestoEntry.Text,
                    Boks = BoxEntry.Text,
                    Težina = TezinaEntry.Text,
                    Vlasnik = VlasnikEntry.Text,
                    Aktivnost = (string)AktivnostPicker.SelectedItem,
                    Id = App.currentCattle.Id,
                    Stari_Vlasnik = StariVlasnikEntry.Text,
                    Datum_Dolaska = Datum_Dolaska_DateLabel.Text != null ? Datum_Dolaska_DateLabel.Text : "0000-00-00",
                    Mjesto_Odlaska = MjestoOdlaskaEntry.Text,
                    Datum_Odlaska = Datum_Odlaska_DateLabel.Text != null ? Datum_Odlaska_DateLabel.Text : "0000-00-00",
                    Način_Odlaska = (string)NačinOdlaskaPicker.SelectedItem
                };

                var json = JsonConvert.SerializeObject(cattle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //HttpClient client = new HttpClient();
                //var result = await client.PutAsync(App.ServerConnection + "/pilotova-farma/api/govedo/update.php", content); //10.0.2.2


                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //var con = "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update.php";

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress  + "/pilotova-farma/api/govedo/update.php", content);
                }

                Cattle cattleLocal = new Cattle() {
                    Broj_Zivotinje = BrojŽivotinjeEntry.Text,
                    Redni_Broj = RedniBrojEntry.Text,
                    Remen = RemenEntry.Text,
                    IKG_Broj = IKGBrojEntry.Text,
                    JIBG = JibgEntry.Text,
                    Spol = (string)GenderPicker.SelectedItem,
                    Rasa = (string)RacePicker.SelectedItem,
                    Uzrast = (string)UzrastPicker.SelectedItem,
                    Datum_Rodenja = Datum_Rođenja_DateLabel.Text != null ? Datum_Rođenja_DateLabel.Text : "0000-00-00",
                    Majka = MajkaEntry.Text,
                    Otac = OtacEntry.Text,
                    Farma = FarmaEntry.Text,
                    Mjesto = MjestoEntry.Text,
                    Boks = BoxEntry.Text,
                    Tezina = TezinaEntry.Text,
                    Vlasnik = VlasnikEntry.Text,
                    Aktivnost = (string)AktivnostPicker.SelectedItem,
                    Id = App.currentCattle.Id,
                    Stari_Vlasnik = StariVlasnikEntry.Text,
                    Datum_Dolaska = Datum_Dolaska_DateLabel.Text != null ? Datum_Dolaska_DateLabel.Text : "0000-00-00",
                    Mjesto_Odlaska = MjestoOdlaskaEntry.Text,
                    Datum_Odlaska = Datum_Odlaska_DateLabel.Text != null ? Datum_Odlaska_DateLabel.Text : "0000-00-00",
                    Nacin_Odlaska = (string)NačinOdlaskaPicker.SelectedItem,
                    icon = App.currentCattle.icon
                };

                //Update Local Databases
                Database.UpdateCattleLocal(cattleLocal);
                for(int i = 0; i < Database.cattleDataSelected.Count; i++) {
                    if (Database.cattleDataSelected[i].Id == cattleLocal.Id) {
                        Database.cattleDataSelected[i] = cattleLocal;
                        //Debug.WriteLine("UPDATED CATTLE FARM: " + Database.cattleDataSelected[i].Farma);
                    }
                }
                for (int i = 0; i < Database.cattleData.Count; i++) {
                    if (Database.cattleData[i].Id == cattleLocal.Id) {
                        Database.cattleData[i] = cattleLocal;
                        //Debug.WriteLine("UPDATED CATTLE FARM: " + Database.cattleDataSelected[i].Farma);
                    }
                }

                //Database.SetCattle();
                await Navigation.PushAsync(new HomePage());
                if (response.IsSuccessStatusCode)
                {
                    await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                }
            }
            catch
            {
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }
        }


        private void RođenjeDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Datum_Rođenja_DateLabel.BindingContext = RođenjeDatePicker;
        }
        private void DolazakDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Datum_Dolaska_DateLabel.BindingContext = DolazakDatePicker;
        }
        private void OdlazakDatePicker_DateSelected(object sender, DateChangedEventArgs e)
        {
            Datum_Odlaska_DateLabel.BindingContext = OdlazakDatePicker;
        }
    }
}