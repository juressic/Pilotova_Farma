using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.CattlePages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Sick : ContentPage
    {
        public Sick()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            List<Cattle> ScikCows = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Bolestan == "DA") {
                    ScikCows.Add(c);
                }
            }

            Sick_Animals.ItemsSource = ScikCows;
        }

        private void Sick_Animals_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }
    }
}