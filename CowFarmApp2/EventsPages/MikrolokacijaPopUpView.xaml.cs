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
    public partial class MikrolokacijaPopUpView : PopupPage {
        public MikrolokacijaPopUpView() {
            InitializeComponent();
        }

        private async void Mikrolokacija_Button_Clicked(object sender, EventArgs e) {
            try {
                Cattle mikrolokacija = new Cattle() {
                    Id = App.currentCattle.Id,
                    Boks = MikrolokacijaEntry.Text
                };

                var json = JsonConvert.SerializeObject(mikrolokacija);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Debug.WriteLine("CONTENT WEIGHT IS: " + json);

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_mikrolokacija.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_mikrolokacija.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_mikrolokacija.php", content);
                }


                /*WeightData weightLocal = new WeightData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Izmjerena_Težina = TezinaEntry.Text,
                    Datum_Pregleda = TezinaDatePicker.Date.ToString("yyyy-MM-dd")  //  DateTime.Today.ToString("yyyy-MM-dd")
                };*/

                //Debug.WriteLine("RESPONSE WEIGHT CREATE" + json);
                //Database.SetMikrolokacijaEventLocal(mikrolokacija);
                //CHANGE SICK STATUS IN LOCALDATABASE
                Cattle cattdata = App.currentCattle;
                cattdata.Boks = MikrolokacijaEntry.Text;
                Database.UpdateCattleLocal(cattdata);

                await DisplayAlert("Uspješno", "Promjena mikrolokacije je zabilježena!", "Ok");
                if (response.StatusCode == System.Net.HttpStatusCode.Created) {
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                }



            } catch {
                Debug.WriteLine("MIKROLOKACIJA FAILEEEED");
                await DisplayAlert("Greška", "Neuspjeli unos nove mikrolokacije!", "Ok");
            }
            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}