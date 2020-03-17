using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using CowFarmApp2.Model;
using Newtonsoft.Json;
using SQLite;
using System.Net;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using System.Linq;

namespace CowFarmApp2
{
    class Database
    {
        public static int lastInsertID;

        //Database list
        public static ObservableCollection<Cattle> cattleData = new ObservableCollection<Cattle>();  //ALL CATTLE
        public static ObservableCollection<Cattle> cattleDataSelected = new ObservableCollection<Cattle>(); //SELECTED FARM CATTLE
        public static ObservableCollection<Cattle> cowsDataSelected = new ObservableCollection<Cattle>(); //COWS ONLY ON SELECTED FARM
        public static ObservableCollection<Cattle> heifersDataSelected = new ObservableCollection<Cattle>(); //COWS ONLY ON SELECTED FARM
        public static ObservableCollection<Cattle> calfsDataSelected = new ObservableCollection<Cattle>(); //COWS ONLY ON SELECTED FARM
        public static ObservableCollection<Cattle> bullsDataSelected = new ObservableCollection<Cattle>(); //COWS ONLY ON SELECTED FARM

        //Database list
        public static ObservableCollection<VeterinarData> veterinarData = new ObservableCollection<VeterinarData>();

        //Database list
        public static ObservableCollection<WeightData> weightData = new ObservableCollection<WeightData>();

        //Database list
        public static ObservableCollection<GravidnostData> gravidnostData = new ObservableCollection<GravidnostData>();

        //Database list
        public static ObservableCollection<AbortusData> abortusData = new ObservableCollection<AbortusData>();

        //Database list
        public static ObservableCollection<StatusData> statusData = new ObservableCollection<StatusData>();

        //Veterinar Data
        //public static ObservableCollection<VeterinarData> currentVeterinarData = new ObservableCollection<VeterinarData>(); //Current selected Button Cow Profile Veterinar Data

        //Current Local Databases Ids
        public static int GovedoCurrentDatabaseId { get; set; }
        public  static int VeterinarCurrentDatabaseId { get; set; }
        public static int WeightCurrentDatabaseId { get; set; }
        public static int GravidnostCurrentDatabaseId { get; set; }
        public static int AbortusCurrentDatabaseId { get; set; }

        public Database()
        {
            //SetCattle();
        }

        public static void Razvrstavanje(string uzrast) {
            switch (uzrast) {
                case "Krava": {
                        cowsDataSelected.Clear();
                        foreach (Cattle c in cattleDataSelected)
                            if(c.Uzrast =="Krava")
                                cowsDataSelected.Add(c);
                        break;
                    }
                case "Tele": {
                        calfsDataSelected.Clear();
                        foreach (Cattle c in cattleDataSelected)
                            if (c.Uzrast == "Tele")
                                calfsDataSelected.Add(c);
                        break;
                    }
                case "Junac":
                case "Junica": {
                        heifersDataSelected.Clear();
                        foreach (Cattle c in cattleDataSelected)
                            if (c.Uzrast == "Junac" || c.Uzrast == "Junica")
                                heifersDataSelected.Add(c);
                        break;
                    }
                case "Bik": {
                        bullsDataSelected.Clear();
                        foreach (Cattle c in cattleDataSelected)
                            if (c.Uzrast == "Bik")
                                bullsDataSelected.Add(c);
                        break;
                    }
            }
        }

        public static void UpdateDatabase()
        {
            SetCattle();
            SetVeterinarEvents();
            SetWeightEvents();
            SetGravidnostEvents();
            SetAbortusEvents();
            //SetStatusEvents();
            #region Old_Code
            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Cattle>(); //just in case CattleData not exist
                //var posts = conn.Table<Post>().ToList();

                foreach (Cattle p in cattleData)
                {
                    conn.Insert(p);
                }

                foreach (Cattle p in cattleData)
                {
                    Debug.WriteLine("This cattle race = " + p.Rasa);
                }



                //int rows = conn.Insert(p);

                //conn.Close();

                //if (rows == 1)
                //{
                //    DisplayAlert("Sucess", "We have one entry in out database", "OK");
                //}
                //else
                //    DisplayAlert("Failed", "We have one entry in out database", "OK");

                //postsData = posts;
                //allAnimalsListView.ItemsSource = cattle; //Binding to List
                //App.selectedCattle = cattle;
            }*/
            #endregion
        }

        #region CATTLE DATA REGION
        public static async void SetCattle()
        {
            Debug.WriteLine("SET CATTLE");
            try
            {
                //var client = new HttpClient(); 

                //SSL CENTRIFICATION
                //ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
                //ServicePointManager.ServerCertificateValidationCallback += (sender, cert, chain, sslPolicyErrors) => true;

                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                string response;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/read.php");
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/read.php");
                }
                
                string result;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/get_auto_increment.php");
                    else
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/govedo/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/govedo/get_auto_increment.php");
                }

                Debug.WriteLine("IDEMOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOOO");
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                GovedoCurrentDatabaseId = int.Parse(autoincrement) - 1;
                Application.Current.Properties["GovedoCurrentDatabaseId"] = GovedoCurrentDatabaseId;

                //LOCAL CONNECTION PHONE 10.0.2.2
                //var response = await client2.GetStringAsync("http://10.0.2.2/pilotova-farma/api/govedo/read.php");

                //Debug.WriteLine("RESPONSE IS: " + response);


                if (response == "{\"message\":\"No Posts Found\"}") {
                    Debug.WriteLine("DATABASE IS EMPTY");
                    Application.Current.Properties["GovedoCurrentDatabaseId"] = 0;
                    cattleData.Clear();
                    using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                        conn.DropTable<Cattle>();
                    }
                        return;
                }

                CattleDataObjects cattleDataObjs = JsonConvert.DeserializeObject<CattleDataObjects>(response);

                //if (cattleDataObjs.Data.Count == 0)
                  //  Debug.WriteLine("DATABASE EMPTY");
                
                //postsData = posts.Data;

                //cattleData.Clear();
                /*cattleData.Clear();
                foreach (Cattle c in cattleDataObjs.Data)
                {
                    cattleData.Add(c);
                    Debug.WriteLine("Broj Životinje Spremljene u Bazu = " + c.Broj_Zivotinje);
                }*/

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.DropTable<Cattle>();
                    //conn.DropTable<CattleData>();
                    //conn.Table<CattleData>().Delete();
                    conn.CreateTable<Cattle>(); //just in case CattleData not exist
                    //var posts = conn.Table<Post>().ToList();
                    //cattleData.Clear();
                    foreach (Cattle c in cattleDataObjs.Data)
                    {
                       switch (c.Uzrast) {
                            case "Krava": {
                                    c.icon = "icons8_cow_96.png";
                                    break;
                                }
                            case "Tele": {
                                    c.icon = "icons8_calf_64.png";
                                    break;
                                }
                            case "Junica": {
                                    c.icon = "icons8_heifer_96.png";
                                    break;
                                }
                            case "Junac": {
                                    c.icon = "icons8_heifer_96.png";
                                    break;
                                }
                            case "Bik": {
                                    c.icon = "icons8_bull_52.png";
                                    break;
                                }
                        }

                        //c.icon = "icons8_cow_96.png";
                        conn.Insert(c);
                        //Debug.WriteLine("Broj Životinje Spremljene u Lokalnu Bazu = " + c.Broj_Zivotinje);
                    }


                    //SET LOCAL DATABASE ICONS
                    cattleData.Clear();
                    lastInsertID = 0;
                    foreach (Cattle c in conn.Table<Cattle>().ToList()) {
                        
                       switch (c.Uzrast) {
                            case "Krava": {
                                    c.icon = "icons8_cow_96.png";
                                    //Debug.WriteLine("COOOOOOOOOOOOOOW");
                                    break;
                                }
                            case "Tele": {
                                    c.icon = "icons8_calf_64.png";
                                    break;
                                }
                            case "Junica": {
                                    c.icon = "icons8_heifer_96.png";
                                    break;
                                }
                            case "Junac": {
                                    c.icon = "icons8_heifer_96.png";
                                    break;
                                }
                            case "Bik": {
                                    c.icon = "icons8_bull_52.png";
                                    break;
                                }
                        }

                        cattleData.Add(c);
                        //Debug.WriteLine("Broj Životinje Spremljene u Bazu = " + c.Broj_Zivotinje);


                        //SET LOCAL DATABASE HIGHEST ID
                        //if (!Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId")) {
                            //if (c.Id > GovedoCurrentDatabaseId) {
                               // GovedoCurrentDatabaseId = c.Id;
                           // }
                        //}
                    }

                    /*Debug.WriteLine("LAST INSET ID = " + GovedoCurrentDatabaseId);
                    if (Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId"))
                        Debug.WriteLine("LAST INSET ID = " + Application.Current.Properties["GovedoCurrentDatabaseId"]);*/

                    /*foreach (Cattle p in cattleData)
                    {
                        Debug.WriteLine("This cattle race = " + p.Rasa);
                    }*/
                }
            }
            catch(Exception e)
            {
                if(Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId"))
                    GovedoCurrentDatabaseId = (int)Application.Current.Properties["GovedoCurrentDatabaseId"];

                Debug.WriteLine("GOVEDO FAAAAAAIIILLLLLLLLLLLL: " + e);
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
                {
                    conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                    var cattle = conn.Table<Cattle>().ToList();
                   // Debug.WriteLine("LOCAL DATABASE COUNT: " + cattle.Count);
                    //List<CattleData> cattleData = new List<CattleData>();
                    //foreach(CattleData data in cattle)
                    //{
                    //    if(data.Spol == "Muški")
                    //    {
                    //        cattleData.Add(data);
                    //    }
                    //}
                    //cattleData = ObservableCollection<Cattle>(new List<Cattle>())
                    cattleData.Clear();
                    foreach(var item in cattle)
                    {
                        cattleData.Add(item);
                    }
                }
            }
            //Listica.ItemsSource = posts.Data;
        }

        public static void GetCattleLocalIds() {

            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                //conn.DropTable<Cattle>();
                conn.CreateTable<Cattle>();
                var cattle = conn.Table<Cattle>().ToList();
                //Debug.WriteLine("LOCAL DATABASE COUNT: " + cattle.Count);
                foreach (Cattle c in conn.Table<Cattle>()) {
                    Debug.WriteLine("LOCAL DATA BASE ID: = " + c.Id);
                }

                if (Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId"))
                    Debug.WriteLine("LAST INSET ID = " + Application.Current.Properties["GovedoCurrentDatabaseId"]);
            }
        }

        /*public static List<Cattle> GetCattleLocal()
        {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                //conn.DropTable<Cattle>();
                conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                var cattle = conn.Table<Cattle>().ToList();
                //List<CattleData> cattleData = new List<CattleData>();
                //foreach(CattleData data in cattle)
                //{
                //    if(data.Spol == "Muški")
                //    {
                //        cattleData.Add(data);
                //    }
                //}
                //foreach (Cattle cd in cattleData)
                    //Debug.WriteLine(cd.name);

                cattleData.Clear();
                foreach(Cattle c in cattle)
                    cattleData.Add(c);

                cattleDataSelected.Clear();
                foreach (Cattle c in cattle)
                    cattleDataSelected.Add(c);


                return cattle;
            }
        }*/

        public static List<Cattle> GetCattleLocal() {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                //conn.DropTable<Cattle>();
                conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                var cattle = conn.Table<Cattle>().ToList();
                //List<CattleData> cattleData = new List<CattleData>();
                //foreach(CattleData data in cattle)
                //{
                //    if(data.Spol == "Muški")
                //    {
                //        cattleData.Add(data);
                //    }
                //}
                //foreach (Cattle cd in cattleData)
                //Debug.WriteLine(cd.name);

                cattleData.Clear();
                foreach (Cattle c in cattle)
                    cattleData.Add(c);

                return cattle;
            }
        }

        public async static void SetCattleLocal(Cattle cattle) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                //var key = conn.ExecuteScalar<int>("SELECT last_insert_rowid() FROM Cattle");
                //cattle.Id = key;

                /*Cattle first = conn.Table<Cattle>().OrderByDescending(p => p.Id).FirstOrDefault();
                int key = first.Id + 1;
                cattle.Id = key;*/

                /*if (Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId")) {
                    Debug.WriteLine("GOVEDO CONTAIN KEY / CATTLE SET");
                    cattle.Id = (int)Application.Current.Properties["GovedoCurrentDatabaseId"] + 1;
                    Application.Current.Properties["GovedoCurrentDatabaseId"] = cattle.Id;
                    await Application.Current.SavePropertiesAsync();
                    GovedoCurrentDatabaseId = (int)Application.Current.Properties["GovedoCurrentDatabaseId"];
                } else {
                    cattle.Id = GovedoCurrentDatabaseId + 1;
                    GovedoCurrentDatabaseId += 1;
                    Application.Current.Properties["GovedoCurrentDatabaseId"] = GovedoCurrentDatabaseId;
                }*/
                cattle.Id = GovedoCurrentDatabaseId + 1;
                GovedoCurrentDatabaseId += 1;
                Application.Current.Properties["GovedoCurrentDatabaseId"] = GovedoCurrentDatabaseId;

                //Databases update
                conn.Insert(cattle);
                cattleData.Add(cattle);
                if(cattle.Mjesto == App.selectedFarm)
                    cattleDataSelected.Add(cattle);
                if(cattle.Mjesto == App.selectedFarm && cattle.Uzrast == "Krava")
                    cowsDataSelected.Add(cattle);
                if (cattle.Mjesto == App.selectedFarm && cattle.Uzrast == "Junac" || cattle.Uzrast == "Junica")
                    heifersDataSelected.Add(cattle);

            }
        }

        public static void UpdateCattleLocal(Cattle cattle) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {

                conn.Update(cattle);
                //string sql = $"UPDATE Cattle SET Bolestan = '{cattle.Bolestan}' WHERE Id={cattle.Id}";
                //conn.Execute(sql);

            }
        }

        public static void DeleteCattleLocal(Cattle cattle) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                //conn.Delete(cattle);
                conn.Table<Cattle>().Delete(x => x.Id == cattle.Id);
                cattleData.Remove(cattle);
                if (cattle.Mjesto == App.selectedFarm)
                    cattleDataSelected.Remove(cattle);
                if (cattle.Mjesto == App.selectedFarm && cattle.Uzrast == "Krava")
                    cowsDataSelected.Remove(cattle);
                if (cattle.Mjesto == App.selectedFarm && cattle.Uzrast == "Junac" || cattle.Uzrast == "Junica")
                    heifersDataSelected.Remove(cattle);
            }
        }
        #endregion

        #region ABORTUS DATA REGION
        public static async void SetAbortusEvents() {
            try {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //ABORTUS READ
                string response = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/abortus/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/abortus/read.php");
                } else if (!App.remoteServerOn) {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/abortus/read.php");
                }
                //AUTO INCREMENT ABORTUS
                string result = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/abortus/get_auto_increment.php");
                    else
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/abortus/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/abortus/get_auto_increment.php");
                }
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                Debug.WriteLine("ABORTUS EVENT AUTOINCREMENT" + autoincrement);
                AbortusCurrentDatabaseId = int.Parse(autoincrement) - 1;
                Application.Current.Properties["AbortusCurrentDatabaseId"] = AbortusCurrentDatabaseId;
                Debug.WriteLine("ABORTUS AUTO INCREMENT: " + (int)Application.Current.Properties["AbortusCurrentDatabaseId"]);

                if (response == "{\"message\":\"No Posts Found\"}") {
                    abortusData.Clear();
                    return;
                }

                AbortusDataObjects abortusDataObjs = JsonConvert.DeserializeObject<AbortusDataObjects>(response);

                abortusData.Clear();
                foreach (AbortusData a in abortusDataObjs.Data) {
                    abortusData.Add(a);
                    Debug.WriteLine("ABORTUS DATA : " + a.Govedo_Id);
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.DropTable<AbortusData>();

                    //Debug
                    /*var cattle = conn.Table<VeterinarData>().ToList();
                    foreach (var item in cattle) {
                        Debug.WriteLine("VETERINAR DATA IN LOCAL DATABASE: " + item.Govedo_Id);
                    }*/

                    conn.CreateTable<AbortusData>(); //just in case CattleData not exist
                    foreach (AbortusData a in abortusDataObjs.Data) {
                        conn.Insert(a);
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("ABORTUS EVENT FAAAAAAAAAAAAIL");
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.CreateTable<AbortusData>(); //just in case CattleDate not exist
                    var cattle = conn.Table<AbortusData>().ToList();
                    abortusData.Clear();
                    foreach (var item in cattle) {
                        abortusData.Add(item);
                    }
                }
            }

        }
        public async static void SetAbortusEventLocal(AbortusData abortus) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<AbortusData>(); //just in case CattleDate not exist

                abortus.Id = AbortusCurrentDatabaseId + 1;
                AbortusCurrentDatabaseId += 1;
                Application.Current.Properties["AbortusCurrentDatabaseId"] = AbortusCurrentDatabaseId;

                conn.Delete(abortus);

                conn.Insert(abortus);
                abortusData.Add(abortus);
            }
        }

        public static void DeleteAbortusLocal(AbortusData abortus) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<AbortusData>(); //just in case CattleDate not exist
                //conn.Delete(cattle);
                conn.Table<AbortusData>().Delete(x => x.Id == abortus.Id);
                //veterinarData.RemoveAt(vet.Id);
                foreach (AbortusData a in abortusData) {
                    if (a.Id == abortus.Id) {
                        abortusData.Remove(a);
                        break;
                    }
                }
            }
        }
        #endregion


        #region VETERINAR DATA REGION
        public static async void SetVeterinarEvents() {
            try {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //VETERINAR READ
                string response = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if(!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/veterinar/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/veterinar/read.php");
                } else if(!App.remoteServerOn) {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/veterinar/read.php");
                }
                //AUTO INCREMENT VETERINAR
                string result = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/veterinar/get_auto_increment.php");
                    else
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/veterinar/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/veterinar/get_auto_increment.php");
                }
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                Debug.WriteLine("VETERINAR EVENT AUTOINCREMENT" + autoincrement);
                VeterinarCurrentDatabaseId = int.Parse(autoincrement) - 1;
                Application.Current.Properties["VeterinarCurrentDatabaseId"] = VeterinarCurrentDatabaseId;
                Debug.WriteLine("VETERINAR AUTO INCREMENT: " + (int)Application.Current.Properties["VeterinarCurrentDatabaseId"]);

                if (response == "{\"message\":\"No Posts Found\"}") {
                    veterinarData.Clear();
                    return;
                }

                VeterinarDataObjects veterinarDataObjs = JsonConvert.DeserializeObject<VeterinarDataObjects>(response);

                veterinarData.Clear();
                foreach (VeterinarData c in veterinarDataObjs.Data) {
                    Debug.WriteLine("ADDED VET DATA: " + c.Diagnoza);
                    veterinarData.Add(c);
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.DropTable<VeterinarData>();

                    //Debug
                    /*var cattle = conn.Table<VeterinarData>().ToList();
                    foreach (var item in cattle) {
                        Debug.WriteLine("VETERINAR DATA IN LOCAL DATABASE: " + item.Govedo_Id);
                    }*/

                    conn.CreateTable<VeterinarData>(); //just in case CattleData not exist
                    foreach (VeterinarData c in veterinarDataObjs.Data) {
                        conn.Insert(c);
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("VETERINAR EVENT FAAAAAAAAAAAAIL");
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.CreateTable<VeterinarData>(); //just in case CattleDate not exist
                    var cattle = conn.Table<VeterinarData>().ToList();
                    veterinarData.Clear();
                    foreach (var item in cattle) {
                        veterinarData.Add(item);
                    }
                }
            }
        }

        public async static void SetVeterinarEventLocal(VeterinarData vet) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<VeterinarData>(); //just in case CattleDate not exist
           
                vet.Id = VeterinarCurrentDatabaseId + 1;
                VeterinarCurrentDatabaseId += 1;
                Application.Current.Properties["VeterinarCurrentDatabaseId"] = VeterinarCurrentDatabaseId;

                conn.Delete(vet); //In Case its already there

                conn.Insert(vet);
                veterinarData.Add(vet);
            }
        }

        public static List<VeterinarData> GetVeterinarEventsLocal() {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<VeterinarData>(); //just in case CattleDate not exist
                var cattle = conn.Table<VeterinarData>().ToList();
                return cattle;
            }
        }

        public static void DeleteVeterinarLocal(VeterinarData vet) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<VeterinarData>(); //just in case CattleDate not exist
                //conn.Delete(cattle);
                conn.Table<VeterinarData>().Delete(x => x.Id == vet.Id);
                //veterinarData.RemoveAt(vet.Id);
                foreach(VeterinarData v in veterinarData) {
                    if(v.Id == vet.Id) {
                        veterinarData.Remove(v);
                        break;
                    }
                }
            }
        }
        #endregion

        #region WEIGHT DATA REGION
        public static async void SetWeightEvents() {
            try {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //WEIGHT READ
                string response = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/tezina/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/tezina/read.php");
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/tezina/read.php");
                }
                Debug.WriteLine("WEIGHT RESPONSE: " + response);

                //AUTO INCREMENT
                string result = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/tezina/get_auto_increment.php");
                    else
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/tezina/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/tezina/get_auto_increment.php");
                }
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                Debug.WriteLine("WEIGHT AUTOINREMENT TEXT: " + autoincrement);
                WeightCurrentDatabaseId = int.Parse(autoincrement) - 1;
                Debug.WriteLine("WEIGHT PROGRESS");
                Application.Current.Properties["WeightCurrentDatabaseId"] = int.Parse(autoincrement) - 1;
                Debug.WriteLine("WEIGHT AUTO INCREMENT: " + (int)Application.Current.Properties["WeightCurrentDatabaseId"]);

                if (response == "{\"message\":\"No Posts Found\"}") {
                    weightData.Clear();
                    return;
                }

                WeightDataObjects weightDataObjs = JsonConvert.DeserializeObject<WeightDataObjects>(response);

                weightData.Clear();
                foreach (WeightData w in weightDataObjs.Data) {
                    Debug.WriteLine("Težina Upisanog Weight Eventa = " + w.Izmjerena_Težina);
                    weightData.Add(w);
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.DropTable<WeightData>();
                    conn.CreateTable<WeightData>(); //just in case CattleData not exist
                    foreach (WeightData w in weightDataObjs.Data) {
                        conn.Insert(w);
                    }
                }
            } catch (Exception e) {
                Debug.WriteLine("WEIGHT FAILEEEEEEEEEEEED");
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.CreateTable<WeightData>(); //just in case CattleDate not exist
                    var weight = conn.Table<WeightData>().ToList();
                    weightData.Clear();
                    foreach (var item in weight) {
                        weightData.Add(item);
                    }
                }
            }
        }

        public async static void SetWeightEventLocal(WeightData weight) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<WeightData>(); //just in case CattleDate not exist

                weight.Id = WeightCurrentDatabaseId + 1;
                WeightCurrentDatabaseId += 1;
                Application.Current.Properties["VeterinarCurrentDatabaseId"] = WeightCurrentDatabaseId;

                conn.Delete(weight);

                conn.Insert(weight);
                weightData.Add(weight);
            }
        }

        public static void DeleteWeightLocal(WeightData weight) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<VeterinarData>(); //just in case CattleDate not exist
                //conn.Delete(cattle);
                conn.Table<VeterinarData>().Delete(x => x.Id == weight.Id);
                //veterinarData.RemoveAt(vet.Id);
                foreach (WeightData v in weightData) {
                    if (v.Id == weight.Id) {
                        weightData.Remove(v);
                        break;
                    }
                }
            }
        }
        #endregion

        #region GRAVIDNOST DATA REGION
        public static async void SetGravidnostEvents() {
            try {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //GRAVIDNOST READ
                string response = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/gravidnost/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/gravidnost/read.php");
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/gravidnost/read.php");
                }

                Debug.WriteLine("GRAVIDNOST RESPONSE: " + response);

                //AUTO INCREMENT GRAVIDNOST
                string result = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/gravidnost/get_auto_increment.php");
                    else
                        result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/gravidnost/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/gravidnost/get_auto_increment.php");
                }
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                GravidnostCurrentDatabaseId = int.Parse(autoincrement);
                Application.Current.Properties["GravidnostCurrentDatabaseId"] = int.Parse(autoincrement);
                //Debug.WriteLine("GRAVIDNOST AUTO INCREMENT: " + (int)Application.Current.Properties["GravidnostCurrentDatabaseId"]);

                if (response == "{\"message\":\"No Posts Found\"}") {
                    gravidnostData.Clear();
                    return;
                }

                GravidnostDataObjects gravidnostDataObjs = JsonConvert.DeserializeObject<GravidnostDataObjects>(response);
                
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.DropTable<GravidnostData>();
                    conn.CreateTable<GravidnostData>(); //just in case CattleData not exist
                    foreach (GravidnostData g in gravidnostDataObjs.Data) {
                        conn.Insert(g);
                    }
                }

                gravidnostData.Clear();
                foreach (GravidnostData g in gravidnostDataObjs.Data) {
                    //Debug.WriteLine("Težina Upisanog Weight Eventa = " + w.Izmjerena_Težina);
                    gravidnostData.Add(g);
                }

            } catch (Exception e) {
                Debug.WriteLine("GRAVIDNOST FAILEEEEEEEEEEEED");
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.CreateTable<GravidnostData>(); //just in case CattleDate not exist
                    var gravidnost = conn.Table<GravidnostData>().ToList();
                    gravidnostData.Clear();
                    foreach (var item in gravidnost) {
                        gravidnostData.Add(item);
                    }
                }
            }
        }

        public static void SetGravidnostEventLocal(GravidnostData gravidnost) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<GravidnostData>(); //just in case GravidnostData not exist

                gravidnost.Id = GravidnostCurrentDatabaseId;
                GravidnostCurrentDatabaseId += 1;
                Application.Current.Properties["GravidnostCurrentDatabaseId"] = GravidnostCurrentDatabaseId;

                Debug.WriteLine("GRAVIDNOST ID = " + GravidnostCurrentDatabaseId);
                conn.Delete(gravidnost);

                conn.Insert(gravidnost);
                gravidnostData.Add(gravidnost);
            }
        }

        public static void DeleteGravidnostLocal(GravidnostData gravidnost) {
            using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                conn.CreateTable<GravidnostData>(); //just in case CattleDate not exist
                //conn.Delete(cattle);
                conn.Table<GravidnostData>().Delete(x => x.Id == gravidnost.Id);
                //veterinarData.RemoveAt(vet.Id);
                foreach (GravidnostData g in gravidnostData) {
                    if (g.Id == gravidnost.Id) {
                        gravidnostData.Remove(g);
                        break;
                    }
                }
            }
        }

        #endregion

        #region STATUSES (FEATURE IN PROGRESS)
        public static async void SetStatusEvents() {
            try {
                HttpClientHandler clientHandler = new HttpClientHandler();
                clientHandler.ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; };
                HttpClient client2 = new HttpClient(clientHandler);

                //WEIGHT READ
                string response = null;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    if (!App.remoteServerTesting)
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/status/read.php");
                    else
                        response = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma-test/api/status/read.php");
                } else {
                    //LOCAL CONNECTION WIN
                    response = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/status/read.php");
                }

                Debug.WriteLine("STATUS RESPONSE: " + response);

                //AUTO INCREMENT
                /*string result;
                if (App.remoteServerOn) {
                    //ONLINE CONNECTION
                    result = await client2.GetStringAsync("https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/tezina/get_auto_increment.php");
                } else {
                    //LOCAL CONNECTION WIN
                    result = await client2.GetStringAsync("http://" + App.localhostAddress + "/pilotova-farma/api/tezina/get_auto_increment.php");
                }
                string autoincrement = string.Join("", result.ToCharArray().Where(Char.IsDigit));
                WeightCurrentDatabaseId = int.Parse(autoincrement) - 1;
                Application.Current.Properties["WeightCurrentDatabaseId"] = int.Parse(autoincrement) - 1;
                Debug.WriteLine("WEIGHT AUTO INCREMENT: " + (int)Application.Current.Properties["WeightCurrentDatabaseId"]);*/

                if (response == "{\"message\":\"No Posts Found\"}") {
                    statusData.Clear();
                    return;
                }

                StatusDataObjects statusDataObjs = JsonConvert.DeserializeObject<StatusDataObjects>(response);

                statusData.Clear();
                foreach (StatusData s in statusDataObjs.Data) {
                    Debug.WriteLine("Težina Upisanog Weight Eventa = " + s.Govedo_Id);
                    statusData.Add(s);
                }

                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.DropTable<StatusData>();
                    conn.CreateTable<StatusData>(); //just in case CattleData not exist
                    foreach (StatusData s in statusDataObjs.Data) {
                        conn.Insert(s);
                    }
                }
            } catch (Exception e) {
                using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation)) {
                    conn.CreateTable<StatusData>(); //just in case CattleDate not exist
                    var status = conn.Table<StatusData>().ToList();
                    statusData.Clear();
                    foreach (var item in status) {
                        statusData.Add(item);
                    }
                }
            }
        }
        #endregion
    }
}
