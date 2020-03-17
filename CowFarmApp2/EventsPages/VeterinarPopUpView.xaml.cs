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

using CowFarmApp2;
using SQLite;

namespace CowFarmApp2.EventsPages {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class VeterinarPopUpView : PopupPage {

        ToolbarItem sickButton;
        public VeterinarPopUpView(ToolbarItem item) {
            InitializeComponent();
            sickButton = item;
        }

        private void ChangeSickButtonText() {
                sickButton.Text = "Nije Bolestan";
        }

        private async void Vet_Button_Clicked(object sender, EventArgs e) {
            //ADD VETERINAR EVENT
            try {
                VeterinarData veterinar = new VeterinarData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Diagnoza = DiagnozaEntry.Text,
                    Liječenje = LijecenjeEntry.Text,
                    Datum_Pregleda = DatumPicker.Date.ToString("yyyy-MM-dd")
                };

                var json = JsonConvert.SerializeObject(veterinar);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //Debug.WriteLine("CONTENT IS: " + json);

                //HttpClient client = new HttpClient();
                //var result = await client.PutAsync("http://" + App.ServerConnection + "/pilotova-farma/api/govedo/create.php", content); //10.0.2.2

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/veterinar/create.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/veterinar/create.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/veterinar/create.php", content);
                }

                /*VeterinarData veterinarLocal = new VeterinarData() {
                    Govedo_Id = App.currentCattle.Id.ToString(),
                    Diagnoza = DiagnozaEntry.Text,
                    Liječenje = LijecenjeEntry.Text,
                    Datum_Pregleda = DatumPicker.Date.ToString("yyyy-MM-dd")
                };*/

                Database.SetVeterinarEventLocal(veterinar);

                await DisplayAlert("Uspješno", "Životinja je označena kao bolesna!", "Ok");
                //if (result.StatusCode == System.Net.HttpStatusCode.Created)
                {
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                }



            } catch {
                //Debug.WriteLine("KOJI KURAC");
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }


            //SET GOVEDO PROFILE BOLESTAN TO DA
            try {
                CattleData SetSick = new CattleData() {
                    Id = App.currentCattle.Id,
                    Bolestan = "DA"
                };

                var json = JsonConvert.SerializeObject(SetSick);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                //HttpClient client = new HttpClient();
                //var result = await client.PutAsync(App.ServerConnection + "/pilotova-farma/api/govedo/update.php", content); //10.0.2.2


                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //var con = "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_sick.php";
                //var result = await client2.PutAsync(App.ServerConnection + "/pilotova-farma/api/govedo/update.php", content); //10.0.2.2

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_sick.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_sick.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_sick.php", content);
                }

                //CHANGE SICK STATUS IN LOCALDATABASE
                Cattle cattdata = App.currentCattle;
                cattdata.Bolestan = "DA";
                Database.UpdateCattleLocal(cattdata);

                //CHANGE PROFILE VARIABLS MAUNAL
                //App.currentCowProfile.ChangeProfileSickStatus(true);

                //Database.SetCattle();
                //await Navigation.PushAsync(new HomePage());
                //if (result.IsSuccessStatusCode) {
                // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                //}
                ChangeSickButtonText();
            } catch {
                await DisplayAlert("Greška", "Možda nema internet, povežite se i pokušajte ponovo!", "Ok");
            }

            await PopupNavigation.Instance.PopAsync(true);
        }
    }
}