﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using Foundation;
using UIKit;

namespace CowFarmApp2.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the 
    // User Interface of the application, as well as listening (and optionally responding) to 
    // application events from iOS.
    [Register("AppDelegate")]
    public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
    {
        //
        // This method is invoked when the application has loaded and is ready to run. In this 
        // method you should instantiate the window, load the UI into it and then make the window
        // visible.
        //
        // You have 17 seconds to return from this method, or iOS will terminate your application.
        //
        public override bool FinishedLaunching(UIApplication app, NSDictionary options)
        {
            global::Xamarin.Forms.Forms.Init();

            string dbName = "travel_db.sqlite";
            string folderPath = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal),"..", "Library");
            string fullPath = Path.Combine(folderPath, dbName);

            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            Rg.Plugins.Popup.Popup.Init(); //Popup init

            App.mobileVersion = true;

            string serverConnection = App.remoteServerOn == true ? "https://pilotova-farma.banovina-agrar.hr/pilotova-farma/api/govedo/read.php" : "http://10.0.2.2/pilotova-farma/api/govedo/read.php";
            LoadApplication(new App(fullPath, serverConnection));

            return base.FinishedLaunching(app, options);
        }
    }
}
