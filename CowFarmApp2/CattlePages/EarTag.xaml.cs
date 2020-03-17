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
    public partial class EarTag : ContentPage
    {
        public EarTag()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            Debug.WriteLine("EarTag Settting");
            List<Cattle> EarTagMissing = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                //Debug.WriteLine("EarTag " + c.Oznaka_Uha);
                if (c.Oznaka_Uha == "NE") {
                    EarTagMissing.Add(c);
                }
            }

            Missing_Ear_Tag.ItemsSource = EarTagMissing;
        }

        private void Missing_Tag_Animal_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }
    }
}