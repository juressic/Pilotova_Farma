using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CowFarmApp2.Model;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.HeiferPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Heifers : ContentPage
    {
        public Heifers()
        {
            InitializeComponent();
            allHeifersListView.ItemsSource = Database.heifersDataSelected;
        }

        protected override void OnAppearing() {
            //Database.SetCattle();
            //BindingContext = this;
            base.OnAppearing();
            /*List<Cattle> allCows = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Uzrast == "Junac" || c.Uzrast == "Junica") {
                    allCows.Add(c);
                }
            }*/
        }

        private void cowsListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }
    }
}