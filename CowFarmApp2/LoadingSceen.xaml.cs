﻿using Rg.Plugins.Popup.Pages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace CowFarmApp2 {
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingSceen : PopupPage {
        public LoadingSceen() {
            InitializeComponent();
        }

        protected override Task OnAppearingAnimationEndAsync() {
            return Content.FadeTo(0.5);
        }

        protected override Task OnDisappearingAnimationBeginAsync() {
            return Content.FadeTo(1);
        }
    }
}