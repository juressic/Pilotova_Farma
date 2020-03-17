using CowFarmApp2.Model;
using Newtonsoft.Json;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.EventsPages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class GravidnostPopUpView : PopupPage {
        public GravidnostPopUpView() {
            InitializeComponent();
        }

        private async void OK_Button_Clicked(object sender, EventArgs e) {

            if (App.currentCattle.Gravidnost != "DA") {

                #region SET GOVEDO GRAVIDNOST TO DA
                try {
                    CattleData Gravidnost = new CattleData() {
                        Id = App.currentCattle.Id,
                        Gravidnost = "DA"
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
                    cattdata.Gravidnost = "DA";
                    Database.UpdateCattleLocal(cattdata);

                    App.currentCowProfile.InitializeBarMenu();
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                    //if (result.IsSuccessStatusCode) {
                    // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                    //}
                } catch {
                    await DisplayAlert("Greška", "Neuspjelo povezivanje s bazom podataka!", "Ok");
                }
                #endregion

                #region SET GRAVIDNOST EVENT
                try {
                    GravidnostData gravidnostData = new GravidnostData() {
                        Govedo_Id = App.currentCattle.Id,
                        //Datum_Začeća = DatumPregledaEntry.Date.ToString("yyyy-MM-dd"),
                        Datum_Začeća = DatumPregledaEntry.Date.AddDays(-int.Parse(DanaGravidnostiEntry.Text)).ToString("yyyy-MM-dd"),
                        //Datum_Pregleda = DateTime.Today.ToString("yyyy-MM-dd")
                        Datum_Pregleda = DatumPregledaEntry.Date.ToString("yyyy-MM-dd")
                    };

                    var json = JsonConvert.SerializeObject(gravidnostData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");


                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                    HttpClient client2 = new HttpClient(clientHandler);

                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        if (!App.remoteServerTesting)
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/gravidnost/create.php", content);
                        else
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/gravidnost/create.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/gravidnost/create.php", content);
                    }

                    GravidnostData gravidnostLocal = new GravidnostData() {
                        Govedo_Id = App.currentCattle.Id,
                        //Datum_Začeća = DatumZaćećaEntry.Date.ToString("yyyy-MM-dd"),
                        Datum_Začeća = DatumPregledaEntry.Date.AddDays(-int.Parse(DanaGravidnostiEntry.Text)).ToString("yyyy-MM-dd"),
                        //Datum_Pregleda = DateTime.Today.ToString("yyyy-MM-dd")
                        Datum_Pregleda = DatumPregledaEntry.Date.ToString("yyyy-MM-dd")
                    };

                    Database.SetGravidnostEventLocal(gravidnostLocal);

                    Debug.WriteLine("CONTENT GRAVIDNOST IS: " + json);

                    await DisplayAlert("Uspješno", "Unos za gravidnost životinje je zabilježen!", "Ok");
                    /*if (response.StatusCode == System.Net.HttpStatusCode.OK) {
                        //Database.SetCattle();
                        //await Navigation.PushAsync(new HomePage());
                    }*/



                } catch {
                    //Debug.WriteLine("KOJI KURAC");
                    await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
                }
                #endregion
                await PopupNavigation.Instance.PopAsync(true);
            }
        }
    }
}