﻿using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using FFImageLoading.Forms.Droid;
using Firebase;
using Plugin.CurrentActivity;

namespace SSBakery.Droid
{
    [Activity(Label = "SSBakery", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            CachedImageRenderer.Init(enableFastRenderer: true);

            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            var options = new FirebaseOptions.Builder()
                .SetApplicationId("<APPLICATION ID>")
                .SetApiKey(Config.ApiKeys.FIREBASE)
                .Build();
            FirebaseApp.InitializeApp(this, options);

            LoadApplication(new App());
        }
    }
}
