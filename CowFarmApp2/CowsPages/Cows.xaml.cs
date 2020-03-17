using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CowFarmApp2.Model;
using SQLite;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.CowsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Cows : ContentPage
    {
        public Cows()
        {
            InitializeComponent();
            allCowsListView.ItemsSource = Database.cowsDataSelected;
        }

        protected override void OnAppearing()
        {
            //Database.SetCattle();
            //BindingContext = this;
            base.OnAppearing();
            //CowsFilter();

            //List<Cattle> allCows = new List<Cattle>();
            //allCows.CollectionChanged;
            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<Cattle>(); //just in case CattleDate not exist
                var cattle = conn.Table<Cattle>().ToList();
                List<Cattle> cattleData = new List<Cattle>();
                foreach(Cattle data in cattle)
                {
                    if(data.Spol == "Female")
                    {
                        cattleData.Add(data);
                    }
                }

                allCowsListView.ItemsSource = cattleData;
                App.selectedCattle = cattleData;
            }*/
        }

        /*public void CowsFilter() {
            Database.allCows.Clear();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Krava" && c.Mjesto == App.selectedFarm) {
                    Database.allCows.Add(c);
                }
            }
        }*/

        private void cowsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);

            /*ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem;
            int index = App.selectedCattle.IndexOf(myItem);
            //DisplayAlert("Sucess", "Item Id = " + index, "OK");

            if (App.selectedCattle[index].Uzrast == "Krava")
            {
                DisplayAlert("Krava", "Saljem karton od krave", "OK");
                CowProfile profile = new CowProfile(index)
                {
                    BrojŽivotinje = App.selectedCattle[index].BrojŽivotinje,
                    Spol = App.selectedCattle[index].Spol,
                    DatumRođenja = App.selectedCattle[index].DatumRođenja
                };
                Navigation.PushAsync(profile);
            }
            if (App.selectedCattle[index].Uzrast == "Tele")
            {
                DisplayAlert("Tele", "Saljem karton od teleta", "OK");
                CowProfile profile = new CowProfile(index)
                {
                    BrojŽivotinje = App.selectedCattle[index].BrojŽivotinje,
                    Spol = App.selectedCattle[index].Spol,
                    DatumRođenja = App.selectedCattle[index].DatumRođenja
                };
                Navigation.PushAsync(profile);
            }
            if (App.selectedCattle[index].Uzrast == "Bik")
            {
                DisplayAlert("Bik", "Saljem karton od bika", "OK");
                BullProfile profile = new BullProfile(index)
                {
                    BrojŽivotinje = App.selectedCattle[index].BrojŽivotinje,
                    Spol = App.selectedCattle[index].Spol,
                    DatumRođenja = App.selectedCattle[index].DatumRođenja
                };
                Navigation.PushAsync(profile);
            }
            //App.selectedCattle[index];
            //Get Data Base
            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CattleData>(); //just in case CattleDate not exist
                var cattle = conn.Table<CattleData>();//.ToList();
                CattleData data = cattle;
                allCowsListView.ItemsSource = cattle;
                App.selectedCattle = cattle;
            }*/

            //Load New Page Profile

            //Set Data Binding*/
        }
    }
}