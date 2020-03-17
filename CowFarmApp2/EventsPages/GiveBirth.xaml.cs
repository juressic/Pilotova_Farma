using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using CowFarmApp2.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;

namespace CowFarmApp2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GiveBirth : ContentPage {
        public GiveBirth() {
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
        }

        private async void Save_Clicked(object sender, EventArgs e) {

            //Stvaranje nove zivotinje
            try {
                CattleData cattle = new CattleData() {
                    Broj_Životinje = BrojŽivotinjeEntry.Text,
                    Redni_Broj = RedniBrojEntry.Text,
                    Remen = RemenEntry.Text,
                    IKG_Broj = IKGBrojEntry.Text,
                    JIBG = JibgEntry.Text,
                    Spol = (string)GenderPicker.SelectedItem,
                    Rasa = (string)RacePicker.SelectedItem,
                    Uzrast = "Tele",
                    Datum_Rođenja = RođenjeDatePicker.Date.ToString("yyyy-MM-dd"),
                    Majka = App.currentCattle.Broj_Zivotinje,
                    Otac = OtacEntry.Text,
                    Farma = App.currentCattle.Farma,
                    Mjesto = App.currentCattle.Mjesto,
                    Boks = App.currentCattle.Boks,
                    Vlasnik = App.currentCattle.Vlasnik,
                    Težina = null,
                    Aktivnost = null,
                    Stari_Vlasnik = null,
                    Datum_Dolaska = null,
                    Mjesto_Odlaska = null,
                    Datum_Odlaska = null,
                    Način_Odlaska = null
                };

                var json = JsonConvert.SerializeObject(cattle);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                Debug.WriteLine("CONTENT BIRTH IS: " + json);

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
                    Uzrast = "Tele",
                    Datum_Rodenja = RođenjeDatePicker.Date.ToString(),
                    Majka = App.currentCattle.Broj_Zivotinje,
                    Otac = OtacEntry.Text,
                    Farma = App.currentCattle.Farma,
                    Mjesto = App.currentCattle.Mjesto,
                    Boks = App.currentCattle.Boks,
                    Vlasnik = App.currentCattle.Vlasnik,
                    Tezina = null,
                    Aktivnost = "DA",
                    Stari_Vlasnik = null,
                    Datum_Dolaska = null,
                    Mjesto_Odlaska = null,
                    Datum_Odlaska = null,
                    Nacin_Odlaska = null
                };
                cattleLocal.icon = "icons8_calf_64.png"; //Add icon code
                Database.SetCattleLocal(cattleLocal);
                Database.GetCattleLocalIds();

                Database.cattleDataSelected.Add(cattleLocal);
                Database.calfsDataSelected.Add(cattleLocal);

                await DisplayAlert("Uspješno", "Nova životinja je dodana na farmu", "Ok");
                //if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //Database.SetCattle();
                    await Navigation.PushAsync(new HomePage());
                }
                Debug.WriteLine("CREATING NEW CALF SUCESSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSSS");
            } catch {

                Debug.WriteLine("CREATING NEW CALF FAILLEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEEED");
                await DisplayAlert("Greška", "Neuspjelo dodavanje novorođene životinje!", "Ok");
            }


            //Ponisti Gravidnost
            if (App.currentCattle.Gravidnost != "NE") {
                //SET GOVEDO GRAVIDNOST TO DA
                try {
                    CattleData Gravidnost = new CattleData() {
                        Id = App.currentCattle.Id,
                        Gravidnost = "NE"
                    };

                    var json = JsonConvert.SerializeObject(Gravidnost);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client2 = new HttpClient(clientHandler);

                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        if (!App.remoteServerTesting)
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_gravidnost.php", content);
                        else
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_gravidnost.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_gravidnost.php", content);
                    }

                    //CHANGE GRAVIDNOST STATUS IN LOCALDATABASE
                    Cattle cattdata = App.currentCattle;
                    cattdata.Gravidnost = "NE";
                    Database.UpdateCattleLocal(cattdata);

                    

                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                    //if (result.IsSuccessStatusCode) {
                    // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                    //}
                } catch {
                    await DisplayAlert("Greška", "Neuspjelo poništavanje gravidnost!", "Ok");
                }

                //Pretvori Junicu u Kravu
                if (App.currentCattle.Uzrast == "Junica") {
                    //SET GOVEDO GRAVIDNOST TO DA
                    try {
                        CattleData Uzrast = new CattleData() {
                            Id = App.currentCattle.Id,
                            Uzrast = "Krava"
                        };

                        var json = JsonConvert.SerializeObject(Uzrast);
                        var content = new StringContent(json, Encoding.UTF8, "application/json");

                        HttpClientHandler clientHandler = new HttpClientHandler();
                        clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                        HttpClient client2 = new HttpClient(clientHandler);

                        HttpResponseMessage response;
                        if (App.remoteServerOn) {
                            //ONLINE CONNECTION
                            if (!App.remoteServerTesting)
                                response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_uzrast.php", content);
                            else
                                response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_uzrast.php", content);
                        } else {
                            //LOCAL CONNECTION WIN
                            response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_uzrast.php", content);
                        }

                        //CHANGE UZRAST STATUS IN LOCALDATABASE
                        Cattle cattdata = App.currentCattle;
                        cattdata.Uzrast = "Krava";
                        Database.UpdateCattleLocal(cattdata);

                        //Database.SetCattle();
                        //await Navigation.PushAsync(new HomePage());
                        //if (result.IsSuccessStatusCode) {
                        // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                        //}
                    } catch {
                        await DisplayAlert("Greška", "Neuspjelo mijenjanje junice u kravu!", "Ok");
                    }
                }
            }
        }
    }
}