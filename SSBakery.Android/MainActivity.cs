using System.Collections.Generic;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Firebase;
using GameCtor.FirebaseAnalytics;
using Plugin.CurrentActivity;
using Rg.Plugins.Popup.Services;

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

            var options = new FirebaseOptions.Builder()
                .SetApplicationId(Config.Constants.CUSTOMER_APP_ID)
                .SetApiKey(Config.ApiKeys.FIREBASE)
                .Build();
            FirebaseApp.InitializeApp(this, options);

            InitCrashlytics();

            Rg.Plugins.Popup.Popup.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);
            //CrossFirebaseAnalytics.Current.SetUserId("123");
            //CrossFirebaseAnalytics.Current.SetUserProperty("propertyName", "propertyValue");
            //CrossFirebaseAnalytics.Current.LogEvent("dummy_event", new Dictionary<string, object>() { { "someKey", "someValue" } });

            Xamarin.Forms.Forms.SetFlags("FastRenderers_Experimental");
            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            LoadApplication(new App());
        }

        public override async void OnBackPressed()
        {
            if (Rg.Plugins.Popup.Popup.SendBackPressed(base.OnBackPressed))
            {
                // Do something if there are some pages in the `PopupStack`
                await PopupNavigation.Instance.PopAsync();
            }
            else
            {
                // Do something if there are not any pages in the `PopupStack`
            }
        }

        private void InitCrashlytics()
        {
            Fabric.Fabric.With(this, new Crashlytics.Crashlytics());

            // Optional: Setup Xamarin / Mono Unhandled exception parsing / handling
            Crashlytics.Crashlytics.HandleManagedExceptions();

            // TODO: Use the current user's information
            // You can call any combination of these three methods
            Crashlytics.Crashlytics.SetUserIdentifier("12345");
            Crashlytics.Crashlytics.SetUserIdentifier("user@fabric.io");
            Crashlytics.Crashlytics.SetUserName("Test User");
        }
    }
}
