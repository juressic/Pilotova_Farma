using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using CowFarmApp2.Model;
using CowFarmApp2.Interfaces;

namespace CowFarmApp2
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePage : TabbedPage
    {

        public HomePage() {
            InitializeComponent();
        }
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            //Database.GetCattleLocalIds();


            //Debug.WriteLine("HOME PAGE CONSTRUCTOR");
            //Database.UpdateDatabase();
        }


        private void SearchButtonPressed(object sender, EventArgs e) {

        }

        

        /*public void SearchBarCattle_TextChanged(object sender, TextChangedEventArgs e) {
            var keyword = SearchBarCattle.Text;
            var suggestions = Database.cattleDataSelected.Where(c => c.Broj_Zivotinje.ToLower().Contains(keyword.ToLower()));
            CattlePages.CowsAll.list.ItemsSource = suggestions;
        }*/
    }
}