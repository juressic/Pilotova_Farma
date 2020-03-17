using CowFarmApp2.EventsPages;
using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using System.Diagnostics;

namespace CowFarmApp2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EventsPage : ContentPage {
        public EventsPage() {
            InitializeComponent();

        }

        protected override void OnAppearing() {
            base.OnAppearing();
            
            List<Event> eventsList = new List<Event>();
            eventsList.Clear();

            foreach (VeterinarData vet in Database.veterinarData)
                Debug.WriteLine("VETERINAR DATA ID: " + vet.Govedo_Id);

            foreach(var vetEvent in Database.veterinarData){
                int govedoIdVet = Int32.Parse(vetEvent.Govedo_Id);
                int govedoId = Int32.Parse(App.currentCattle.Id.ToString());
                if (govedoIdVet == govedoId) {
                    Debug.WriteLine("LIJECENJE = " +vetEvent.Liječenje);
                    vetEvent.Icon = "icons8_veterinar_96.png";
                    vetEvent.Datum = vetEvent.Datum_Pregleda;
                    eventsList.Add(vetEvent);
                }
            }

            foreach (var weightEvent in Database.weightData) {
                int govedoIdWeight = Int32.Parse(weightEvent.Govedo_Id);
                int govedoId = Int32.Parse(App.currentCattle.Id.ToString());
                if (govedoIdWeight == govedoId) {
                    weightEvent.Icon = "icons8_weight_52.png";
                    weightEvent.Datum = weightEvent.Datum_Pregleda;
                    eventsList.Add(weightEvent);
                }
            }

            foreach (var gravidnostEvent in Database.gravidnostData) {
                int govedoIdGravidnost = gravidnostEvent.Govedo_Id;
                int govedoId = Int32.Parse(App.currentCattle.Id.ToString());
                if (govedoIdGravidnost == govedoId) {
                    gravidnostEvent.Icon = "icons8_pregnant_100.png";
                    gravidnostEvent.Datum = gravidnostEvent.Datum_Pregleda;
                    eventsList.Add(gravidnostEvent);
                }
            }

            foreach (var abortusEvent in Database.abortusData) {
                int govedoIdAbortus = abortusEvent.Govedo_Id;
                int govedoId = Int32.Parse(App.currentCattle.Id.ToString());
                if (govedoIdAbortus == govedoId) {
                    //abortusEvent.Icon = "icons8_pregnant_100.png";
                    abortusEvent.Datum = DateTime.Today.ToString("yyyy-MM-dd");
                    eventsList.Add(abortusEvent);
                }
            }

            foreach (var a in eventsList) {
                Debug.WriteLine("EVENT DATE IS: " + a.Datum);
            }
            List<Event> SortedList = eventsList.OrderByDescending(o => DateTime.Parse(o.Datum)).ToList();
            
            allEventsListView.ItemsSource = SortedList; //BINDING CONTEXT
            Debug.WriteLine("RAZABIRANJE EVENTOVA");
        }

        private void allEventsListView_ItemTapped(object sender, ItemTappedEventArgs e) {
            ListView myListView = (ListView)sender;

            //VETERINAR EVENT CLICKED
            if(myListView.SelectedItem.GetType() == typeof(VeterinarData)) {
                //SET CURRENT VETERINAR DATA
                var myItem = myListView.SelectedItem as VeterinarData;
                App.currentEvent = myItem;
               
                myListView.SelectedItem = null;  //Remove selection mark
                Navigation.PushAsync(new VeterinarEventPage()); //Push to Veterinar Page
            }
            //WEIGHT EVENT CLICKED
            else if (myListView.SelectedItem.GetType() == typeof(WeightData)) {
                //SET CURRENT VETERINAR DATA
                var myItem = myListView.SelectedItem as WeightData;
                App.currentEvent = myItem;

                myListView.SelectedItem = null;  //Remove selection mark
                Navigation.PushAsync(new TezinaEventPage()); //Push to Veterinar Page
                //Debug.WriteLine("Težina Za Ovaj Event = " + myItem.Izmjerena_Težina);
            }
            //GRAVIDNOST EVENT CLICKED
            else if (myListView.SelectedItem.GetType() == typeof(GravidnostData)) {
                //SET CURRENT VETERINAR DATA
                var myItem = myListView.SelectedItem as GravidnostData;
                App.currentEvent = myItem;
                Debug.WriteLine("CURRENT EVENT ID = " + App.currentEvent.Id);
                //Debug.WriteLine("Datum Zaceca = " + myItem.Datum_Začeća);
                myListView.SelectedItem = null;  //Remove selection mark
                Navigation.PushAsync(new GravidnostEventPage()); //Push to Veterinar Page
                //Debug.WriteLine("Težina Za Ovaj Event = " + myItem.Izmjerena_Težina);
            }
            //ABORTUS EVENT CLICKED
            else if (myListView.SelectedItem.GetType() == typeof(AbortusData)) {
                //SET CURRENT ABORTUS DATA
                var myItem = myListView.SelectedItem as AbortusData;
                App.currentEvent = myItem;
                Debug.WriteLine("CURRENT EVENT ID = " + App.currentEvent.Id);
                //Debug.WriteLine("Datum Zaceca = " + myItem.Datum_Začeća);
                myListView.SelectedItem = null;  //Remove selection mark
                Navigation.PushAsync(new AbortusEventPage()); //Push to Veterinar Page
                //Debug.WriteLine("Težina Za Ovaj Event = " + myItem.Izmjerena_Težina);
            }


        }
    }
}