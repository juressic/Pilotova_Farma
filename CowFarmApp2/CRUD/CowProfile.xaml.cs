using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using SQLite;
using CowFarmApp2.Model;
using Newtonsoft.Json;
using System.Net.Http;
using System.Diagnostics;
using CowFarmApp2.EventsPages;

using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CowProfile : ContentPage
    {
       
        public int IdNumber;

        public List <ToolbarItem> toolbarItems = new List<ToolbarItem>();
        public CowProfile()
        {
            InitializeComponent();
            //IdNumber = ID;

            //List<Cattle> blabla = Database.GetCattleLocal();

            BindingContext = App.currentCattle;
            //BindingContext = this;

            
            InitializeBarMenu();
            //ADD TOOLBAR ITEMS
            /*foreach (ToolbarItem item in toolbarItems) {
                ToolbarItems.Add(item);
            }*/

            //POTOMCI
            App.currentCattle.potomci.Clear();
            foreach(Cattle c in Database.cattleData) {
                if (c.Majka == App.currentCattle.Broj_Zivotinje)
                    App.currentCattle.potomci.Add(c);
            }

            IzmjeniBtn.IsEnabled = App.enableEventButtons;
            
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            //Reprodukcija.Text = "Reprodukcija";

            //SICK STATUS TEXT
            /*if (App.currentCattle.Bolestan == "DA")
                SickToolBar.Text = "Nije Bolestan";
            else
                SickToolBar.Text = "Bolestan";

            //EARTAG STATUS TEXT
            if(App.currentCattle.Oznaka_Uha == "NE") {
                EarTagBar.Text = "Oznaka Dodana";
            } else {
                EarTagBar.Text = "Nedostaje Oznaka";
            }*/

            //DisplayAlert("Number Found", "Item Number = " + BrojŽivotinje, "OK");
            //Broj_Životinje.Text = BrojŽivotinje;

            //GET EVENTS...
            //GetVeterinarEvents();
        }

        private ToolbarItem Bolestan;
        private ToolbarItem Oznaka;

        private ToolbarItem Gravidnost;
        private ToolbarItem Rođenje;
        private ToolbarItem Abortus;

        public void InitializeBarMenu() {
            Debug.WriteLine("CLEARRRRRRRRRRRRRRRRING");
            ToolbarItems.Clear();
            toolbarItems.Clear();
            //REPRODUKCIJA
            Gravidnost = new ToolbarItem(); Gravidnost.Text = "Gravidnost"; Gravidnost.Order = ToolbarItemOrder.Secondary; Gravidnost.Clicked += Event_Gravidnost_Clicked; Gravidnost.IsEnabled = App.enableEventButtons;
            Rođenje = new ToolbarItem(); Rođenje.Text = "Rođenje"; Rođenje.Order = ToolbarItemOrder.Secondary; Rođenje.Clicked += Event_Rođenje_Clicked; Rođenje.IsEnabled = App.enableEventButtons;
            Abortus = new ToolbarItem(); Abortus.Text = "Abortus"; Abortus.Order = ToolbarItemOrder.Secondary; Abortus.Clicked += Event_Abortus_Clicked; Abortus.IsEnabled = App.enableEventButtons;
            Bolestan = new ToolbarItem(); Bolestan.Order = ToolbarItemOrder.Secondary; Bolestan.Clicked += Event_Bolestan_Clicked; Bolestan.IsEnabled = App.enableEventButtons;
            if (App.currentCattle.Bolestan == "DA")
                Bolestan.Text = "Nije Bolestan";
            else
                Bolestan.Text = "Bolestan";
            Oznaka = new ToolbarItem(); Oznaka.Text = "Nema Oznaku"; Oznaka.Order = ToolbarItemOrder.Secondary; Oznaka.Clicked += Event_OznakaUha_Clicked; Oznaka.IsEnabled = App.enableEventButtons;
            if (App.currentCattle.Oznaka_Uha != "NE")
                Oznaka.Text = "Nema Oznaku";
            else
                Oznaka.Text = "Ima Oznaku";
            ToolbarItem Težina = new ToolbarItem(); Težina.Text = "Izmjerena Težina"; Težina.Order = ToolbarItemOrder.Secondary; Težina.Clicked += Event_Tezina_Clicked; Težina.IsEnabled = App.enableEventButtons;
            ToolbarItem Odbačen = new ToolbarItem(); Odbačen.Text = "Odbačen"; Odbačen.Order = ToolbarItemOrder.Secondary; Odbačen.IsEnabled = false;
            ToolbarItem Smrt = new ToolbarItem(); Smrt.Text = "Smrt"; Smrt.Order = ToolbarItemOrder.Secondary; Smrt.IsEnabled = false;
            ToolbarItem Izbriši = new ToolbarItem(); Izbriši.Text = "Izbriši"; Izbriši.Order = ToolbarItemOrder.Secondary; Izbriši.Clicked += Event_Delete_Cattle; Izbriši.IsEnabled = App.enableEventButtons;
            ToolbarItem Mikrolokacija = new ToolbarItem(); Mikrolokacija.Text = "Promjena Mikrolokacije"; Mikrolokacija.Order = ToolbarItemOrder.Secondary; Mikrolokacija.Clicked += Event_Promjena_Mikrolokacije; Mikrolokacija.IsEnabled = App.enableEventButtons;

            if (App.currentCattle.Uzrast == "Krava" || App.currentCattle.Uzrast == "Junica") {
                if (App.currentCattle.Gravidnost != "DA")
                    toolbarItems.Add(Gravidnost);

                toolbarItems.Add(Rođenje);
                toolbarItems.Add(Abortus);
                /*if (App.currentCattle.Gravidnost == "DA") {
                    toolbarItems.Add(Rođenje);
                    toolbarItems.Add(Abortus);
                }*/
            }

            toolbarItems.Add(Bolestan);
            toolbarItems.Add(Oznaka);
            toolbarItems.Add(Težina);
            toolbarItems.Add(Mikrolokacija);
            toolbarItems.Add(Odbačen);
            toolbarItems.Add(Smrt);
            toolbarItems.Add(Izbriši);

            foreach (ToolbarItem item in toolbarItems) {
                ToolbarItems.Add(item);
            }
        }

        /*public async void GetVeterinarEvents() {
            try {

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                string response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/veterinar/read.php");
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/veterinar/read.php");
                }

                #region DATABASE EMPTY
                if (response == "{\"message\":\"No Posts Found\"}") {
                    Debug.WriteLine("DATABASE IS EMPTY");
                    Database.currentVeterinarData.Clear();
                    return;
                }
                #endregion
                VeterinarDataObjects veterinarDataObjs = JsonConvert.DeserializeObject<VeterinarDataObjects>(response);

                Database.currentVeterinarData.Clear();
                foreach (VeterinarData c in veterinarDataObjs.Data) {
                    Database.currentVeterinarData.Add(c);
                }
            } catch (Exception e) {
                await DisplayAlert("Greška", "Događaji Veterinara se nisu učitali", "Ok");
            }
        }*/

        async void Event_Delete_Cattle(object sender, EventArgs e) {
            bool answer = await DisplayAlert("Pitanje?", "Želiš li stvarno izbrisati ovu životinju?", "Da", "Ne");
            Debug.WriteLine("Answer: " + answer);
            if(answer) {
                Delete_CowProfile(sender, e);
            } else {
                return;
            }
        }

        private async void Delete_CowProfile(object sender, EventArgs e)
        {
            try
            {
                /*Cattle cattle = new Cattle()
                {
                    Id = App.currentCattle.Id
                };*/

                Database.GetCattleLocalIds();
                var json = JsonConvert.SerializeObject(App.currentCattle);

                var content = new StringContent(json, Encoding.UTF8, "application/json");


                //HttpClient client = new HttpClient();
                //var result = await client.PutAsync("http://" + App.ServerConnection + "/pilotova-farma/api/govedo/delete.php", content); //10.0.2.2

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                HttpResponseMessage response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/delete.php", content);
                    else
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/delete.php", content);
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/delete.php", content);
                }

                //DELETE CATTLE LOCALLY
                Database.DeleteCattleLocal(App.currentCattle);

                for(int i = 0; i < Database.cattleDataSelected.Count; i++) {
                    if (Database.cattleDataSelected[i].Id == App.currentCattle.Id) {
                        Database.cattleDataSelected.RemoveAt(i);
                        Debug.WriteLine("DELETED FROM SELECTED CATTLE");
                   }
                }
                
                Database.Razvrstavanje(App.currentCattle.Uzrast);
                await DisplayAlert("Uspješno", "Životinja je izbrisana iz baze podataka!", "Ok");
                Debug.WriteLine("Delete Success");
                //Database.SetCattle();
                await Navigation.PushAsync(new HomePage());
                if (response.IsSuccessStatusCode)
                {
                    //Debug.WriteLine("Delete Success");
                }
            }
            catch(Exception exception)
            {
                Debug.WriteLine("LOL: " + exception);
                await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
            }
        }



        private async void Edit_CowProfile(object sender, EventArgs args)
        {
            await Navigation.PushAsync(new UpdateCattle());
        }

        /*public int Remen { get; set; }

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
        
        public string DatumDolaska { get; set; }*/


        private void Events_Button_Clicked(object sender, EventArgs e) {
            EventsPage events = new EventsPage();

            Navigation.PushAsync(events);
        }

        private void Calfs_Button_Clicked(object sender, EventArgs e) {
            CalfsInfoPage calfs = new CalfsInfoPage();

            Navigation.PushAsync(calfs);
        }

        private async void Event_Bolestan_Clicked(object sender, EventArgs e) {

            if(App.currentCattle.Bolestan != "DA" || App.currentCattle.Bolestan == null) {
                await PopupNavigation.Instance.PushAsync(new VeterinarPopUpView(Bolestan));
            } else {
                //UNSET SICK
                try {
                    CattleData SetSick = new CattleData() {
                        Id = App.currentCattle.Id,
                        Bolestan = "NE"
                    };

                    Cattle cattdata = App.currentCattle;
                    cattdata.Bolestan = "NE";
                    Database.UpdateCattleLocal(cattdata);
                    Debug.WriteLine("CATTLE LOCAL BOLESTAN SET TO NE");

                    foreach(Cattle c in Database.GetCattleLocal()) {
                        Debug.WriteLine("COW BOLESTAN STATUS = " + c.Bolestan);
                    }

                    var json = JsonConvert.SerializeObject(SetSick);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client2 = new HttpClient(clientHandler);

                    //var con = "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_sick.php";


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
                    await DisplayAlert("Uspješno", "Životinja je označena kao zdrava!", "Ok");
                    //CHANGE PROFILE VARIABLS MAUNAL
                    //ChangeProfileSickStatus(false);
                    //Debug.WriteLine("SETTING SICK BUTTON TEXT");
                    Bolestan.Text = "Bolestan";
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                    //if (result.IsSuccessStatusCode) {
                    // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                    //}
                } catch {
                    await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
                }
            }
        }

        private async void Event_OznakaUha_Clicked(object sender, EventArgs e) {

            if (App.currentCattle.Oznaka_Uha == "NE" ) {
                //SET GOVEDO EARTAG TO DA
                try {
                    CattleData OznakaUha = new CattleData() {
                        Id = App.currentCattle.Id,
                        Oznaka_Uha = "DA"
                    };

                    var json = JsonConvert.SerializeObject(OznakaUha);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client2 = new HttpClient(clientHandler);

                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        if (!App.remoteServerTesting)
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_ear_tag.php", content);
                        else
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_ear_tag.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_ear_tag.php", content);
                    }

                    //CHANGE SICK STATUS IN LOCALDATABASE
                    Cattle cattdata = App.currentCattle;
                    cattdata.Oznaka_Uha = "DA";
                    Database.UpdateCattleLocal(cattdata);

                    //CHANGE PROFILE VARIABLS MAUNAL
                    Oznaka.Text = "Nema Oznaku";


                    await DisplayAlert("Uspješno", "Životinja je označena!", "Ok");
                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                    //if (result.IsSuccessStatusCode) {
                    // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                    //}
                } catch {
                    await DisplayAlert("Greška", "Možda nema internet, povežite se i pokušajte ponovo!", "Ok");
                }
            } else {
                //UNSET EARTAG
                try {
                    CattleData OznakaUha = new CattleData() {
                        Id = App.currentCattle.Id,
                        Oznaka_Uha = "NE"
                    };

                    Cattle cattdata = App.currentCattle;
                    cattdata.Oznaka_Uha = "NE";
                    Database.UpdateCattleLocal(cattdata);
                    Debug.WriteLine("CATTLE LOCAL BOLESTAN SET TO NE");

                    foreach (Cattle c in Database.GetCattleLocal()) {
                        Debug.WriteLine("COW BOLESTAN STATUS = " + c.Bolestan);
                    }

                    var json = JsonConvert.SerializeObject(OznakaUha);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client2 = new HttpClient(clientHandler);

                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        if (!App.remoteServerTesting)
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_ear_tag.php", content);
                        else
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/update_ear_tag.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_ear_tag.php", content);
                    }

                    //CHANGE PROFILE VARIABLS MAUNAL
                    Oznaka.Text = "Ima Oznaku";

                    await DisplayAlert("Uspješno", "Životinja je neoznačena!", "Ok");
                } catch {
                    await DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
                }
            }
        }

        /*public void ChangeProfileSickStatus(bool isSick) {
            if(isSick == true) {
                App.currentCattle.Bolestan = "DA";
                SickToolBar.Text = "Nije Bolestan";
            } else {
                App.currentCattle.Bolestan = "NE";
                SickToolBar.Text = "Bolestan";
            }
        }*/

        private async void Event_Tezina_Clicked(object sender, EventArgs e) {
            await PopupNavigation.Instance.PushAsync(new IzmjerenaTezinaPopUp(TezinaLabel));
                //PopupNavigation.Instance.PushAsync(new IzmjerenaTezinaPopUp());
            /*} else {
                //UNSET SICK
                try {
                    CattleData SetSick = new CattleData() {
                        Id = App.currentCattle.Id.ToString(),
                        Bolestan = "NE"
                    };

                    Cattle cattdata = App.currentCattle;
                    cattdata.Bolestan = "NE";
                    Database.UpdateCattleLocal(cattdata);
                    Debug.WriteLine("CATTLE LOCAL BOLESTAN SET TO NE");

                    foreach (Cattle c in Database.GetCattleLocal()) {
                        Debug.WriteLine("COW BOLESTAN STATUS = " + c.Bolestan);
                    }

                    var json = JsonConvert.SerializeObject(SetSick);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };
                    HttpClient client2 = new HttpClient(clientHandler);

                    //var con = "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_sick.php";


                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/update_sick.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/update_sick.php", content);
                    }

                    //CHANGE PROFILE VARIABLS MAUNAL
                    ChangeProfileSickStatus(false);

                    //Database.SetCattle();
                    //await Navigation.PushAsync(new HomePage());
                    //if (result.IsSuccessStatusCode) {
                    // await DisplayAlert("Uspješno", "Vaša izmjena je zabilježena", "Ok");
                    //}
                } catch {
                    DisplayAlert("Greška", "Nema internet konekcije, povežite se i pokušajte ponovo!", "Ok");
                }
            }*/
        }

        private async void Event_Gravidnost_Clicked(object sender, EventArgs e) {

                await PopupNavigation.Instance.PushAsync(new GravidnostPopUpView());
                InitializeBarMenu();
        }

        private async void Event_Rođenje_Clicked(object sender, EventArgs e) {

            if (App.currentCattle.Gravidnost != "NE") 
            {
                //PUSH PAGE

                //SET GOVEDO GRAVIDNOST TO NE
                /*try {
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
                    await DisplayAlert("Greška", "Neuspjelo povezivanje s bazom podataka!", "Ok");
                }*/

                Navigation.PushAsync(new GiveBirth());
            }
        }

        private async void Event_Abortus_Clicked(object sender, EventArgs e) {

            //if (App.currentCattle.Gravidnost != "NE") 
            {
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
                    //}
                    //toolbarItems.Clear();
                    InitializeBarMenu();
                    /*foreach (ToolbarItem item in toolbarItems) {
                        ToolbarItems.Add(item);
                    }*/
                } catch {
                    await DisplayAlert("Greška", "Neuspjelo povezivanje s bazom podataka!", "Ok");
                }

                //SET EVENT
                try {
                    AbortusData abortusData = new AbortusData() {
                        Govedo_Id = App.currentCattle.Id,
                        Datum_Abortusa = DateTime.Today.ToString("yyyy-MM-dd")
                    };

                    var json = JsonConvert.SerializeObject(abortusData);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    //Debug.WriteLine("CONTENT WEIGHT IS: " + json);

                    HttpClientHandler clientHandler = new HttpClientHandler();
                    clientHandler.ServerCertificateCustomValidationCallback = (o, cert, chain, sslPolicyErrors) => { return true; };  //Certification avoiding
                    HttpClient client2 = new HttpClient(clientHandler);

                    HttpResponseMessage response;
                    if (App.remoteServerOn) {
                        //ONLINE CONNECTION
                        if (!App.remoteServerTesting)
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/abortus/create.php", content);
                        else
                            response = await client2.PutAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/abortus/create.php", content);
                    } else {
                        //LOCAL CONNECTION WIN
                        response = await client2.PutAsync("http://" + App.localhostAddress + "/pilotova-farma/api/abortus/create.php", content);
                    }


                    /*WeightData weightLocal = new WeightData() {
                        Govedo_Id = App.currentCattle.Id.ToString(),
                        Izmjerena_Težina = TezinaEntry.Text,
                        Datum_Pregleda = TezinaDatePicker.Date.ToString("yyyy-MM-dd")  //  DateTime.Today.ToString("yyyy-MM-dd")
                    };*/

                    Debug.WriteLine("ABORTED");
                    Database.SetAbortusEventLocal(abortusData);

                    await DisplayAlert("Uspješno", "Unos događaja za abortus je zabilježen", "Ok");

                    if (response.StatusCode == System.Net.HttpStatusCode.Created) {
                        //Database.SetCattle();
                        //await Navigation.PushAsync(new HomePage());
                        //await DisplayAlert("Pozdrav", "Vaš unos Abortusa je zabilježen u bazi podataka!", "Ok");
                    }



                } catch {
                    Debug.WriteLine("WTF abortus FAILLEEED");
                    await DisplayAlert("Greška", "Vaš unos događaja za abortus nije unesen!", "Ok");
                }
            }
        }

        private async void Event_Promjena_Mikrolokacije(object sender, EventArgs e) {
            await PopupNavigation.Instance.PushAsync(new MikrolokacijaPopUpView());
        }

        /*public void ChangeProfileEarTagStatus(bool tag_missing) {
            if (tag_missing == true) {
                App.currentCattle.Oznaka_Uha = "DA";
                EarTagBar.Text = "Nedostaje Oznaka";
            } else {
                App.currentCattle.Oznaka_Uha = "NE";
                EarTagBar.Text = "Oznaka Dodana";
            }
        }*/
    }
}