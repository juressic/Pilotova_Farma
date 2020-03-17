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
    public partial class IzmjerenaTezinaPopUp : PopupPage {
        public IzmjerenaTezinaPopUp() {
            InitializeComponent();
        }

        private async void Weight_Button_Clicked(object sender, EventArgs e) {
            //ADD EVENT
            try {
                WeightData weightData = new WeightData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Izmjerena_Težina = TezinaEntry.Text,
                    Datum_Pregleda = TezinaDatePicker.Date.ToString("yyyy-MM-dd")
                };

                var json = JsonConvert.SerializeObject(weightData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Debug.WriteLine("CONTENT WEIGHT IS: " + json);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/tezina/create.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/tezina/create.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/tezina/create.php", content);
                }


                /*WeightData weightLocal = new WeightData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Izmjerena_Težina = TezinaEntry.Text,
                    Datum_Pregleda = TezinaDatePicker.Date.ToString("yyyy-MM-dd")  //  DateTime.Today.ToString("yyyy-MM-dd")
                };*/

                //Debug.WriteLine("RESPONSE WEIGHT CREATE" + json);
                Database.SetWeightEventLocal(weightData);

                await DisplayAlert("Uspješno", "Unos izmjene težine je zabilježen!", "Ok");
                if (response.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                }



            } catch {
                Debug.WriteLine("KOJI KURAC");
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }

            //UPDATE GOVEDO
            try {
                CattleData cattleData = new CattleData() {
                    Id = App.currentCattle.Id,
                    Težina = TezinaEntry.Text
                };

                var json = JsonConvert.SerializeObject(cattleData);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Debug.WriteLine("CONTENT WEIGHT IS: " + json);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_tezina.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_tezina.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_tezina.php", content);
                }


                /*WeightData weightLocal = new WeightData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Izmjerena_Težina = TezinaEntry.Text,
                    Datum_Pregleda = TezinaDatePicker.Date.ToString("yyyy-MM-dd")  //  DateTime.Today.ToString("yyyy-MM-dd")
                };*/

                //Debug.WriteLine("RESPONSE WEIGHT CREATE" + json);
                Cattle cattdata = App.currentCattle;
                cattdata.Tezina = TezinaEntry.Text;
                Database.UpdateCattleLocal(cattdata);

                //await DisplayAlert("Uspješno", "Unos izmjene težine je zabilježen!", "Ok");
                if (response.StatusCode == System.Net.HttpStatusCode.Created) {
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                }



            } catch {
                Debug.WriteLine("KOJI KURAC");
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }

            await PopupNavigation.Instance.PopAsync(true);
        }

    }
}