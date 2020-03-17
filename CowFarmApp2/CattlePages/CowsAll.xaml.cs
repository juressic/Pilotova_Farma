using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using CowFarmApp2.Model;

using MySql.Data;
using MySql.Data.MySqlClient;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Xamarin.Forms.Internals;
using System.Diagnostics;
using System.Collections.Specialized;
using System.Collections.ObjectModel;

namespace CowFarmApp2.CattlePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CowsAll : ContentPage
    {

        //public static ObservableCollection<Cattle> list;
        public static ListView list;
        //List<CattleData> selectedCattle = new List<CattleData>();
        public CowsAll()
        {
            InitializeComponent();
            list = allAnimalsListView;
            /*foreach(var cattle in allAnimalsListView.ItemsSource) {
                var myItem = cattle as Cattle;
                list.Add(myItem);
            }*/
            /*Database.SetCattle();
            BindingContext = this;
            allAnimalsListView.ItemsSource = Database.GetCattle();
            Debug.WriteLine("Cows All Constructor");*/
            allAnimalsListView.ItemsSource = Database.cattleDataSelected;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            //Debug.WriteLine("Message from CowsAll OnAppearing");
            //Database.SetCattle();
            //allAnimalsListView.ItemsSource = Database.GetCattle();
            //Debug.WriteLine("Binding With CowsAll");



            //allAnimalsListView.ItemsSource = Database.GetCattleLocal();

            //LOCAL DATABASE
            /*using (SQLiteConnection conn = new SQLiteConnection(App.DatabaseLocation))
            {
                conn.CreateTable<CattleData>(); //just in case CattleDate not exist
                var cattle = conn.Table<CattleData>().ToList();
                //List<CattleData> cattleData = new List<CattleData>();
                //foreach(CattleData data in cattle)
                //{
                //    if(data.Spol == "Muški")
                //    {
                //        cattleData.Add(data);
                //    }
                //}

                allAnimalsListView.ItemsSource = cattle;
                App.selectedCattle = cattle;
            }*/

            //ONLINE DATABASE
            /*using (MySqlConnection mySqlCon = new MySqlConnection(App.connectionString))
            {
                //List of all cows
                
                //Bind to Xaml list
            }*/

        }

        //EVENT ON PROFILE ITEM SELECT
        private void allCowsListView_ItemTapped(object sender, ItemTappedEventArgs e)
        {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            Debug.WriteLine("Icon for this cow is: " + myItem.icon);
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            App.currentCowProfile = profile;

            // = new NavigationPage(profile);
            Navigation.PushAsync(profile);
            //profile.BindingContext = myItem;


            //int index = App.selectedCattle.IndexOf(myItem);

            //Debug.WriteLine("index is = " + index);

            //DisplayAlert("Sucess", "Item Id = " + index, "OK");

            /*if(App.selectedCattle[index].Uzrast == "Krava")
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
            }*/
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

            //Set Data Binding
        }

        private void SearchButtonPressed(object sender, EventArgs e)
        {
            
        }

        private void SearchBarCattle_TextChanged(object sender, TextChangedEventArgs e)
        {
            var keyword = SearchBarCattle.Text;


            //var suggestions = Database.cattleData.Where(c => c.Broj_Zivotinje.ToLower().Contains(keyword.ToLower()));
            var suggestions = Database.GetCattleLocal().Where(c => c.Broj_Zivotinje.ToLower().Contains(keyword.ToLower()));
            //var s = from c in Database.cattleData where c.Broj_Zivotinje.Contains(keyword) select c;

            allAnimalsListView.ItemsSource = suggestions;
        }
    }
}