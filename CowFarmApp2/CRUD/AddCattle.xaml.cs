using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using CowFarmApp2.Model;
using SQLite;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Data;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AddCattle : ContentPage
    {
        public AddCattle()
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
        }

        private async void Save_Clicked(object sender, EventArgs e)
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
                    Vlasnik = VlasnikEntry.Text,
                    Težina = TezinaEntry.Text,
                    Aktivnost = (string)AktivnostPicker.SelectedItem,
                    Stari_Vlasnik = StariVlasnikEntry.Text,
                    Datum_Dolaska = Datum_Dolaska_DateLabel.Text != null ? Datum_Dolaska_DateLabel.Text : "0000-00-00",
                    Mjesto_Odlaska = MjestoOdlaskaEntry.Text,
                    Datum_Odlaska = Datum_Odlaska_DateLabel.Text != null ? Datum_Odlaska_DateLabel.Text : "0000-00-00",
                    Način_Odlaska = (string)NačinOdlaskaPicker.SelectedItem
                };

                var json = JsonConvert.SerializeObject(cattle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("CONTENT IS: " + json);

                //HttpClient client = new HttpClient();
                //var result = await client.PutAsync("http://" + App.ServerConnection + "/pilotova-farma/api/govedo/create.php", content); //10.0.2.2

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                HttpClient client2 = new HttpClient(clientHandler);
            
                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/create.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/create.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/create.php", content);
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
                    Vlasnik = VlasnikEntry.Text,
                    Tezina = TezinaEntry.Text,
                    Aktivnost = (string)AktivnostPicker.SelectedItem,
                    Stari_Vlasnik = StariVlasnikEntry.Text,
                    Datum_Dolaska = Datum_Dolaska_DateLabel.Text != null ? Datum_Dolaska_DateLabel.Text : "0000-00-00",
                    Mjesto_Odlaska = MjestoOdlaskaEntry.Text,
                    Datum_Odlaska = Datum_Odlaska_DateLabel.Text != null ? Datum_Odlaska_DateLabel.Text : "0000-00-00",
                    Nacin_Odlaska = (string)NačinOdlaskaPicker.SelectedItem
                };
                Database.SetCattleLocal(cattleLocal);
                Database.GetCattleLocalIds();

                Database.cattleDataSelected.Add(cattleLocal);

                //if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //Database.SetCattle();
                    await Navigation.PushAsync(new HomePage());
                    //await DisplayAlert("Pozdrav", "Vaš unos je zabilježen u bazi podataka!", "Ok");
                }
            }
            catch
            {
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }

            //-------------------------------------------------------------------------------------------------------

            //LOCAL DATABASE
            //Adding to database
            /*CattleData cattle = new CattleData()
            {
                Remen = int.Parse(RemenEntry.Text),
                BrojŽivotinje = BrojŽivotinjeEntry.Text,
                Spol = (string)GenderPicker.SelectedItem,
                Rasa = (string)RacePicker.SelectedItem,
                Uzrast = (string)UzrastPicker.SelectedItem,
                DatumRođenja = RođenjeDatePicker.Date.ToString(),
                //Migracija_KPUG = MigracijaDatePicker.Date.ToString(),
                Majka = MajkaPicker.Text,
                Otac = OtacPicker.Text,
                //Isporuka = IsporukaDatePicker.Date.ToString(),

                //DatumDolaska = DolazakDatePicker.Date.ToString()
            };

            SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation);
            conn.CreateTable<CattleData>();
            int rows = conn.Insert(cattle);

            conn.Close();

            if(rows == 1)
            {
                DisplayAlert("Sucess", "We have one entry in out database", "OK");
            }else
                DisplayAlert("Failed", "We have one entry in out database", "OK");
                
             */

            //string connectionString = @"localhost;Database=pilotovafarma;Uid=root;Pwd=;";
            /*using(MySqlConnection mySqlCon = new MySqlConnection(App.connectionString))
            {
                mySqlCon.Open();
                MySqlCommand mySqlCmd = new MySqlCommand("CowAdd", mySqlCon);
                mySqlCmd.CommandType = CommandType.StoredProcedure;
                mySqlCmd.Parameters.AddWithValue("_Remen", RemenEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_BrojŽivotinje", BrojŽivotinjeEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_Spol", (string)GenderPicker.SelectedItem);
                mySqlCmd.Parameters.AddWithValue("_Rasa", (string)RacePicker.SelectedItem);
                mySqlCmd.Parameters.AddWithValue("_Uzrast", (string)UzrastPicker.SelectedItem);
                mySqlCmd.Parameters.AddWithValue("_DatumRođenja", RođenjeDatePicker.Date);
                mySqlCmd.Parameters.AddWithValue("_Majka", MajkaEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_Otac", OtacEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_Farma", FarmaEntry.Text );
                mySqlCmd.Parameters.AddWithValue("_Mjesto", MjestoEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_Box", BoxEntry.Text);

                //Dolazak Na gospodarstvo
                mySqlCmd.Parameters.AddWithValue("_Datum_Dolaska", DolazakDatePicker.Date);
                mySqlCmd.Parameters.AddWithValue("_Stari_Vlasnik", StariVlasnikEnrty.Text);

                //Odlazak Sa Gospodarstva
                mySqlCmd.Parameters.AddWithValue("_Mjesto_Odlaska", MjestoOdlaskaEntry.Text);
                mySqlCmd.Parameters.AddWithValue("_Datum_Odlaska", OdlazakDatePicker.Date);
                mySqlCmd.Parameters.AddWithValue("_Način_Odlaska", (string)NačinOdlaskaPicker.SelectedItem);*/
            /*switch (NačinOdlaskaPicker.Items[NačinOdlaskaPicker.SelectedIndex])
            {
                case "PRODAJA":
                    mySqlCmd.Parameters.AddWithValue("_Datum_Prodaja", OdlazakDatePicker.Date);
                    break;
                case "KLANJE":
                    mySqlCmd.Parameters.AddWithValue("_Datum_Klanje", OdlazakDatePicker.Date);
                    break;
                case "UGINUĆE":
                    mySqlCmd.Parameters.AddWithValue("_Datum_Uginuće", OdlazakDatePicker.Date);
                    break;
                case "KRAĐA-GUBITAK":
                    mySqlCmd.Parameters.AddWithValue("_Datum_Gubitak", OdlazakDatePicker.Date);
                    break;
            }*/
            //_Govedo_ID_DNG

            /*mySqlCmd.ExecuteNonQuery();
            DisplayAlert("Submitted Sucessful for Online Database", "Yes", "OK");
        }*/
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