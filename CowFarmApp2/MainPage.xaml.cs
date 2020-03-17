using CowFarmApp2.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace CowFarmApp2
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
            Database.UpdateDatabase();
        }

        private void LogIn_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new HomePage());
            //Navigation.PushAsync(new MasterDetailMenu());
            
            App.Current.MainPage = new MasterDetailMenu();

            /*foreach (Cattle c in Database.GetCattleLocal()) {
                Debug.WriteLine("COW BOLESTAN STATUS = " + c.Bolestan);
            }*/
        }
    }
}
