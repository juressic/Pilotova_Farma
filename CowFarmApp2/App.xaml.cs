using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using System.Collections.Generic;
using CowFarmApp2.Model;
using System.Diagnostics;
using System.Linq;

using CowFarmApp2.Interfaces;

namespace CowFarmApp2
{
    public partial class App : Application
    {

        public static App current;

        #region Old Connection Strings for MySQL Orcale Nudget Package
        //ONLINE DATABASE CONNECTION STRING
        /*static string server_Remote = "172.24.20.160";
        static string database_Remote = "pilotovafarma";
        static string uid_Remote = "jmiskov";
        static string password_Remote = "VfWMOSq4O6Lp";
        public static string connectionString_Remote = "SERVER=" + server_Remote + ";" + "DATABASE=" + database_Remote + ";" + "UID=" + uid_Remote + ";" + "PASSWORD=" + password_Remote + ";*/

        //LOCAL DATABASE CONNECTION STRING
        /*static string server = "localhost";
        static string database = "pilotovafarma";
        static string uid = "root";
        static string password = "";
        public static string connectionString = "SERVER=" + server + ";" + "DATABASE=" + database + ";" + "UID=" + uid + ";" + "PASSWORD=" + password + ";";*/
        #endregion

        public static bool remoteServerOn = true; //Switch between remote and local connection link
        public static bool remoteServerTesting = true;

        public static bool enableEventButtons = true;

        public static string localhostAddress = "localhost"; //10.0.2.2 / localhost

        //LOCAL DATABASE
        public static string DatabaseLocation = string.Empty;  //Location in memory for storing SQLite

        //API GOVEDO READ
        public static List<Cattle> selectedCattle = new List<Cattle>();  //Current Getted Cattle List

        //CURRENT CATTLE
        public static Cattle currentCattle; //Current selected Button in cattle list
        public static CowProfile currentCowProfile; //Current selected Button Cow Profile

        //EVENTS
        //public static List<VeterinarData> currentVeterinarData = new List<VeterinarData>(); //Current selected Button Cow Profile Veterinar Data
        //public static VeterinarData currentVeterinarEvent;
        //public static WeightData currentWeightEvent;
        public static Event currentEvent;

        //CURRENT SELECED FARM
        public static string selectedFarm;

        //SERVER CONNECTION
        public static string ServerConnection = string.Empty;  //Connection Link (could be localhost or https://...


        public App()
        {
            InitializeComponent();
            current = this;
            MainPage = new MainPage();

            
        }

        public App(string databaseLocation, string serverConnection)
        {
            InitializeComponent();

            MainPage = new NavigationPage(new MainPage());

            //MainPage = new MainPage();

            DatabaseLocation = databaseLocation;

            ServerConnection = serverConnection;
        }

        //PROMJENA FARME
        public static void FarmPickerChange(Picker farmPicker) {

            //CowFarmApp2.HomePage.current.SearchBarCattle_TextChanged(new object(), new TextChangedEventArgs(null, null));
            App.selectedFarm = (string)farmPicker.SelectedItem;
            Database.cattleDataSelected.Clear();
            Database.cowsDataSelected.Clear();
            Database.heifersDataSelected.Clear();
            Database.calfsDataSelected.Clear();
            Database.bullsDataSelected.Clear();
            foreach (Cattle c in Database.cattleData) {
                if (farmPicker.SelectedIndex == 0) { //ALL COWS
                    //SVE ŽIVOTINJE
                    Database.cattleDataSelected.Add(c);
                    //KRAVE UPDATE
                    if (c.Uzrast == "Krava")
                        Database.cowsDataSelected.Add(c);
                    //JUNCI UPDATE
                    if (c.Uzrast == "Junac" || c.Uzrast == "Junica")
                        Database.heifersDataSelected.Add(c);
                    //TELCI UPDATE
                    if (c.Uzrast == "Tele")
                        Database.calfsDataSelected.Add(c);
                    //BIKOVI UPDATE
                    if (c.Uzrast == "Bik")
                        Database.bullsDataSelected.Add(c);
                } 
                
                else if (farmPicker.SelectedIndex == 3) {
                    //SVE ŽIVOTINJE
                    if (c.Mjesto != "BAIR" && c.Mjesto != "KRIČKE")
                        Database.cattleDataSelected.Add(c);
                    //KRAVE UPDATE
                    if(c.Mjesto != "BAIR" && c.Mjesto != "KRIČKE" && c.Uzrast == "Krava")
                        Database.cowsDataSelected.Add(c);
                    //JUNCI UPDATE
                    if (c.Mjesto != "BAIR" && c.Mjesto != "KRIČKE" && (c.Uzrast == "Junac" || c.Uzrast == "Junica"))
                        Database.heifersDataSelected.Add(c);
                    //TELCI UPDATE
                    if (c.Mjesto != "BAIR" && c.Mjesto != "KRIČKE" && c.Uzrast == "Tele")
                        Database.calfsDataSelected.Add(c);
                    //BIKOVI UPDATE
                    if (c.Mjesto != "BAIR" && c.Mjesto != "KRIČKE" && c.Uzrast == "Bik")
                        Database.bullsDataSelected.Add(c);
                } 
                
                else if (c.Mjesto == App.selectedFarm) {
                    //SVE ŽIVOTINJE
                    Database.cattleDataSelected.Add(c);
                    //KRAVE UPDATE
                    if (c.Uzrast == "Krava")
                        Database.cowsDataSelected.Add(c);
                    //JUNCI UPDATE
                    if (c.Uzrast == "Junac" || c.Uzrast == "Junica")
                        Database.heifersDataSelected.Add(c);
                    //TELCI UPDATE
                    if (c.Uzrast == "Tele")
                        Database.calfsDataSelected.Add(c);
                    //JUNCI UPDATE
                    if (c.Uzrast == "Bik")
                        Database.bullsDataSelected.Add(c);
                }
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts



            /*if (Application.Current.Properties.ContainsKey("GovedoCurrentDatabaseId")) {
                Debug.WriteLine("GOVEDO ID HAS KEY");
                Database.GovedoCurrentDatabaseId = (int)Application.Current.Properties["GovedoCurrentDatabaseId"];
            }*/
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
