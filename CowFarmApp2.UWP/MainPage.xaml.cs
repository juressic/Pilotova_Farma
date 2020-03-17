using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace CowFarmApp2.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            //ServicePointManager.ServerCertificateValidationCallback = (message, certificate, chain, sslPolicyErrors) => true;

            //string dbName = "travel_db.sqlite";
            string folderPath = "Windows.Storage.ApplicationData.Current.LocalFolder.Path";
            //string fullPath = Path.Combine(folderPath, dbName);

            Rg.Plugins.Popup.Popup.Init();

            //String for server address
            string serverConnection = CowFarmApp2.App.remoteServerOn == true ? 
                                    "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/read.php" :
                                    "http://localhost/pilotova-farma/api/govedo/read.php";
            //string serverConnection = "pilotova-farma.banovina-agrar.hr";
            //string serverConnection = "localhost";
            LoadApplication(new CowFarmApp2.App(folderPath, serverConnection));
        }
    }
}
