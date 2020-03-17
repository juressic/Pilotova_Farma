﻿using CowFarmApp2.Model;
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
    public partial class HeiferFemale : ContentPage
    {
        public HeiferFemale()
        {
            InitializeComponent();
        }

        protected override void OnAppearing() {
            base.OnAppearing();

            //Debug.WriteLine("EarTag Settting");
            List<Cattle> FemaleHeifersList = new List<Cattle>();
            foreach (Cattle c in Database.cattleDataSelected) {
                //Debug.WriteLine("EarTag " + c.Oznaka_Uha);
                if (c.Uzrast == "Junica") {
                    FemaleHeifersList.Add(c);
                }
            }

            FemaleHeifersListView.ItemsSource = FemaleHeifersList;
        }

        private void FemaleHeifersListView_ItemTapped(object sender, ItemTappedEventArgs e) {
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