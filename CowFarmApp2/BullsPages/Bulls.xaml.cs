using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CowFarmApp2.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.BullsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Bulls : ContentPage
    {
        public Bulls()
        {
            InitializeComponent();
            BullsListView.ItemsSource = Database.bullsDataSelected;
        }

        protected override void OnAppearing() {
            //Database.SetCattle();
            //BindingContext = this;
            base.OnAppearing();
            Debug.WriteLine("BULLLLS");
            foreach (Cattle c in Database.bullsDataSelected)
                Debug.WriteLine("BULL: " + c.Broj_Zivotinje);


            /*List<Cattle> allCows = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Bik") {
                    allCows.Add(c);
                }
            }*/
            //allBullsListView.ItemsSource = allCows;
        }

        private void BullsListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }

    }
}