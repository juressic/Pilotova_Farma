using CowFarmApp2.Model;
using Rg.Plugins.Popup.Services;
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
        public static MainPage current;

        public MainPage()
        {
            InitializeComponent();
            current = this;
            Database.UpdateDatabase();
        }

        private async void LogIn_Clicked(object sender, EventArgs e)
        {
            //Navigation.PushAsync(new HomePage());
            //Navigation.PushAsync(new MasterDetailMenu());

            //var a = Database.UpdateDatabase();

            //if(a.IsCompleted)
                App.Current.MainPage = new MasterDetailMenu();

            /*foreach (Cattle c in Database.GetCattleLocal()) {
                Debug.WriteLine("COW BOLESTAN STATUS = " + c.Bolestan);
            }*/
        }

        /*private async void LoadingBtnClicked(object sender, EventArgs args)
        {
            await PopupNavigation.Instance.PushAsync(new LoadingSceen());
            await Task.Delay(2000);
            MessagingCenter.Send<MainPage>(this, "Hi");
        }*/
    }

}
