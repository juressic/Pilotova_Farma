using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.HeiferPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HeiferMale : ContentPage
    {
        public HeiferMale()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            //Debug.WriteLine("EarTag Settting");
            List<Cattle> MaleHeifersList = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                //Debug.WriteLine("EarTag " + c.Oznaka_Uha);
                if (c.Uzrast == "Junac") {
                    MaleHeifersList.Add(c);
                }
            }

            MaleHeifersListView.ItemsSource = MaleHeifersList;

        }

        private void MaleHeifersListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            App.currentCowProfile = profile;

            Navigation.PushAsync(profile);
        }
    }
}