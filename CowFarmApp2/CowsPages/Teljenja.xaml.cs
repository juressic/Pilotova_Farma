using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2.CowsPages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class Teljenja : ContentPage
    {

        public Teljenja()
        {
            InitializeComponent();
            /*foreach(GravidnostData gravidnost in Database.gravidnostData) {
                if(gravidnost.Govedo_Id == App.currentCattle.Id.ToString()) {
                    gravidnostData = gravidnost;
                }
            }*/

            List<Cattle> Teljenja = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                if (c.Gravidnost == "DA") {

                    foreach (GravidnostData gd in Database.gravidnostData) {
                        if(gd.Govedo_Id == c.Id) {
                            c.datumZačeća = gd.Datum_Začeća;

                            DateTime dt = DateTime.Parse(gd.Datum_Začeća);
                            c.remaningDays = (int)((dt.AddMonths(9) - DateTime.Today).TotalDays);
                            c.datumTeljenja = dt.AddMonths(9).ToString("yyyy-MM-dd");
                            //Debug.WriteLine("DATUM TELENJA = " + dt.AddMonths(9).ToString());
                        }
                    }
                    Teljenja.Add(c);
                }
            }
            var gravidnostSorted = Teljenja.OrderBy(x => x.remaningDays).ToList();
            Teljenja_List.ItemsSource = gravidnostSorted;
            BindingContext = this;
        }

        private void Teljenja_List_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;
            var myItem = myListView.SelectedItem as Cattle;
            App.currentCattle = myItem;

            myListView.SelectedItem = null;

            CowProfile profile = new CowProfile();

            Navigation.PushAsync(profile);
        }
    }
}