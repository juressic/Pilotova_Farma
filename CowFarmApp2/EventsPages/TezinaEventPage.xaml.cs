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

namespace CowFarmApp2.EventsPages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TezinaEventPage : ContentPage {
        public TezinaEventPage() {
            InitializeComponent();
            BindingContext = App.currentEvent;
        }

        private async void ToolbarItem_Clicked(object sender, EventArgs e) {
            try {
                WeightData weight = new WeightData() {
                    Id = App.currentEvent.Id
                };

                var json = JsonConvert.SerializeObject(weight);

                var content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/tezina/delete.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/tezina/delete.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/tezina/delete.php", content);
                }

                Database.DeleteWeightLocal(weight);

                //Database.SetCattle();
                //await Navigation.PushAsync(new EventsPage());
                await Navigation.PopAsync();
                if (response.IsSuccessStatusCode) {
                    Debug.WriteLine("Delete Weight Event Success");
                }
            } catch (Exception exception) {
                Debug.WriteLine("LOL: " + exception);
                await DisplayAlert("Greška", "Neuspjelo povezivanje s bazom podataka!", "Ok");
            }
        }
    }
}