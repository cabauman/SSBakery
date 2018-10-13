﻿using System;
using SSBakery.Config;
using SSBakeryAdmin.UI.Modules;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SSBakeryAdmin
{
    public partial class App : Application
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SYNC_FUSION_LICENSE);

            InitializeComponent();

            var bootstrapper = new AppBootstrapper();
            MainPage = new MainPage(bootstrapper.CreateMainViewModel(), bootstrapper.NavigationShell);
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
