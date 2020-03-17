using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using SQLite;

using MySql.Data;
using MySql.Data.MySqlClient;
using CowFarmApp2.CattlePages;
using CowFarmApp2.Model;
using System.Diagnostics;
using System.Collections.Specialized;
using System.IO;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterDetailMenu : MasterDetailPage
    {
        public MasterDetailMenu()
        {
            InitializeComponent();

            
            FarmPicker.Items.Add("SVE FARME");
            FarmPicker.Items.Add("BAIR");
            FarmPicker.Items.Add("KRIČKE");
            FarmPicker.Items.Add("BEZ FARME");
            FarmPicker.SelectedIndex = 0;

            //Database.UpdateDatabase();
            CattleCountShow();
            NavigationPage.SetHasBackButton(this, false);
            
            Detail = new NavigationPage(new HomePage());
            //Detail = new HomePage();

            Debug.WriteLine("MASTER DETAIL PAGE");
            //Database.cattleData.CollectionChanged += CattleCountShow;
            Database.cattleDataSelected.CollectionChanged += CattleCountShow;


        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //CattleCountShow();
            //Debug.WriteLine("MASTER DETAIL MENU CALL FROM ON APPEARING");
            

        }
        private void FarmPicker_SelectedIndexChanged(object sender, EventArgs e) {

            App.FarmPickerChange(FarmPicker);
            CattleCountShow();
            /*if (FarmPicker.SelectedItem == "Sve Farme") {

            } else if(FarmPicker.SelectedItem == "Bair") {

            } else if (FarmPicker.SelectedItem == "Kričke") {

            }*/

        }

        //MAster detail side panel count animals
        public void CattleCountShow(object sender, NotifyCollectionChangedEventArgs args)
        {
            //All Animals
            //List<Cattle> allCattle = Database.cattleData.Count;
            AnimalsCount.Text = Database.cattleDataSelected.Count.ToString();

            
            //All Cows
            List<Cattle> allCows = new List<Cattle>();
            foreach(Cattle c in Database.cattleDataSelected)
            {
                if(c.Uzrast == "Krava")
                {
                    allCows.Add(c);
                }
            }
            CowsCount.Text = allCows.Count().ToString();

            //All Bulls
            List<Cattle> allBulls = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected)
            {
                if (c.Uzrast == "Bik")
                {
                    allBulls.Add(c);
                }
            }
            BullsCount.Text = allBulls.Count().ToString();

            //All Heithers
            List<Cattle> allHeifer = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected)
            {
                if (c.Uzrast == "Junac" || c.Uzrast == "Junica")
                {
                    allHeifer.Add(c);
                }
            }
            HeiferCount.Text = allHeifer.Count().ToString();

            //All Calfs
            List<Cattle> allCalfs = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected)
            {
                if (c.Uzrast == "Tele")
                {
                    allCalfs.Add(c);
                }
            }
            CalfsCount.Text = allCalfs.Count().ToString();
        }

        //MAster detail side panel count animals manually
        public void CattleCountShow() {
            //All Animals
            //List<Cattle> allCattle = Database.cattleData.Count;
            AnimalsCount.Text = Database.cattleDataSelected.Count.ToString();


            //All Cows
            List<Cattle> allCows = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Krava") {
                    allCows.Add(c);
                }
            }
            CowsCount.Text = allCows.Count().ToString();
            //CowsCount.Text = Database.cowsDataSelected.Count().ToString();

            //Debug.WriteLine("ALL COWS COUNT = " + allCows.Count().ToString());

            //All Bulls
            List<Cattle> allBulls = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Bik") {
                    allBulls.Add(c);
                }
            }
            BullsCount.Text = allBulls.Count().ToString();

            //All Heithers
            List<Cattle> allHeifer = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Junac" || c.Uzrast == "Junica") {
                    allHeifer.Add(c);
                }
            }
            HeiferCount.Text = allHeifer.Count().ToString();

            //All Calfs
            List<Cattle> allCalfs = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Tele") {
                    allCalfs.Add(c);
                }
            }
            CalfsCount.Text = allCalfs.Count().ToString();
        }

        /*public void UpdateSidePanel()
        {

            using (MySqlConnection mySqlCon = new MySqlConnection(App.connectionString))
            {
                mySqlCon.Open();
                MySqlCommand CattleCount = new MySqlCommand("Count_Rows_Govedo", mySqlCon);
                string cattleCount = CattleCount.ExecuteScalar().ToString();
                AnimalsCount.Text = cattleCount;

                MySqlCommand allCowsCount = new MySqlCommand("Count_Krave", mySqlCon);
                string cowCount = allCowsCount.ExecuteScalar().ToString();
                CowsCount.Text = cowCount;

                MySqlCommand allCalfsCount = new MySqlCommand("Count_Tele", mySqlCon);
                string calfsCount = allCowsCount.ExecuteScalar().ToString();
                CalfsCount.Text = calfsCount;

                MySqlCommand allHeiferCount = new MySqlCommand("Count_Heifer", mySqlCon);
                string heiferCount = allHeiferCount.ExecuteScalar().ToString();
                HeiferCount.Text = heiferCount;

                MySqlCommand allBullsCount = new MySqlCommand("Count_Bulls", mySqlCon);
                string bullsCount = allBullsCount.ExecuteScalar().ToString();
                BullsCount.Text = bullsCount;

            }
        }*/

        private void Životinje_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HomePage());
            //Detail = new HomePage();
            //Navigation.PushAsync(new HomePage());
            IsPresented = false;
        }

        private void Krave_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new CowsPage());
            //Detail = new CowsPage();
            IsPresented = false;
        }

        private void Junci_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new HeiferPage());
            IsPresented = false;
        }

        private void Telići_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new CalfsPage());
            IsPresented = false;
        }

        private void Odbijeni_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new WeanedCalfsPage());
            IsPresented = false;
        }

        private void Bikovi_Clicked(object sender, EventArgs e)
        {
            Detail = new NavigationPage(new BullsPage());
            IsPresented = false;
        }
        private void AddCattle_Clicked(object sender, EventArgs e)
        {
            //Detail = new NavigationPage(new AddCattle());
            Detail.Navigation.PushAsync(new AddCattle());
            IsPresented = false;
        }

        private void Stada_Clicked(object sender, EventArgs e)
        {

        }

        private void LogOutView_Clicked(object sender, EventArgs e)
        {
            App.Current.MainPage = new MainPage();
        }

        private void CSV_Clicked(object sender, EventArgs e) {
            string fileName = Path.Combine("/storage/emulated/0/Android/data/com.VirtualAnt.PilotovaFarma/", "cattle7.csv");
            //string fileName = Path.Combine(Environment.GetFolderPath., "cattle7.csv");
            //File.WriteAllText(fileName, "Hello World , Ono Tvoje ,\n ko je, a ja");
            string naslovi = "Id, Broj Životinje, Redni Broj, Remen";
            string nastavakVeterinar;
            string zivotinje = null;

            try {
                foreach(Cattle cattle in Database.cattleData)
                    zivotinje += cattle.Id + ", " + cattle.Broj_Zivotinje + ", " + cattle.Redni_Broj + ", " + cattle.Remen + ",\n";

                File.WriteAllText(fileName, naslovi + " ,\n" + zivotinje);
                DisplayAlert("Napomena", "Datoteka je spremljena.", "OK");
            } catch {
                DisplayAlert("Pogreška", "Spremanje datoteke nije uspjelo.", "OK");
            }
        }

        private void Mikrolokacije_Clicked(object sender, EventArgs e) {
            
        }
    }
}