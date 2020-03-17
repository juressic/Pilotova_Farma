using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CalfsInfoPage : ContentPage {
        public CalfsInfoPage() {
            InitializeComponent();

            potomciListView.ItemsSource = App.currentCattle.potomci;
        }
        
        private void potomciListView_ItemTapped(object sender, ItemTappedEventArgs e) {

            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            App.currentCowProfile = profile;

            // = new NavigationPage(profile);
            Navigation.PushAsync(profile);
        }
    }
}