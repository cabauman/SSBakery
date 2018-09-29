using Android.App;
using Android.Content.PM;
using Android.OS;
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

            var options = new FirebaseOptions.Builder()
                .SetApplicationId(Config.Constants.CUSTOMER_APP_ID)
                .SetApiKey(Config.ApiKeys.FIREBASE)
                .Build();
            FirebaseApp.InitializeApp(this, options);

            InitCrashlytics();
            FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);

            Xamarin.Forms.Forms.Init(this, bundle);
            Xamarin.Auth.Presenters.XamarinAndroid.AuthenticationConfiguration.Init(this, bundle);
            Xamarin.FormsMaps.Init(this, bundle);
            CrossCurrentActivity.Current.Init(this, bundle);

            LoadApplication(new App());
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
