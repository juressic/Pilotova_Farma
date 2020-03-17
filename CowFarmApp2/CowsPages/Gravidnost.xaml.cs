using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.CowsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Gravidnost : ContentPage
    {
        public Gravidnost()
        {
            InitializeComponent();
            
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            BindingContext = this;
            List<Cattle> Gravidne = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Gravidnost == "DA") {
                    Gravidne.Add(c);
                }
            }
            Gravidnost_List.ItemsSource = Gravidne;
        }

        private void Gravidnost_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }
    }
}