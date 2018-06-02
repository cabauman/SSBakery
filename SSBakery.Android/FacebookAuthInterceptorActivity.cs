using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace SSBakery.Droid
{
    [Activity(Label = "FacebookAuthInterceptorActivity")]
    [
        IntentFilter
        (
            actions: new[] { Intent.ActionView },
            Categories = new[]
            {
                    Intent.CategoryDefault,
                    Intent.CategoryBrowsable
            },
            //DataSchemes = new[]
            //{
            //    FacebookAuthConfig.DATA_SCHEME,
            //},
            DataHosts = new[]
            {
                "authorize",
            },
            DataPaths = new[]
            {
                "/",
            }
        )
    ]
    public class FacebookAuthInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Convert Android.Net.Url to Uri
            var uri = new Uri(Intent.Data.ToString());

            // Load redirectUrl page
            //AuthenticationState.Authenticator.OnPageLoading(uri);

            Finish();
        }
    }
}
