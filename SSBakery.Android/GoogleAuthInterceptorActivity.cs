using System;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;

namespace SSBakery.Droid
{
    [Activity(Label = "GoogleAuthInterceptorActivity")]
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
            //    // First part of the redirect url
            //    GoogleAuthConfig.DATA_SCHEME_ANDROID,
            //},
            DataPaths = new[]
            {
                "/oauth2redirect"
            }
        )
    ]
    public class GoogleAuthInterceptorActivity : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            global::Android.Net.Uri uri_android = Intent.Data;

#if DEBUG
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            sb.AppendLine("GoogleAuthInterceptorActivity.OnCreate()");
            sb.Append("     uri_android = ").AppendLine(uri_android.ToString());
            System.Diagnostics.Debug.WriteLine(sb.ToString());
#endif

            var uri = new Uri(uri_android.ToString());

            // Send the URI to the Authenticator for continuation
            //SignInActivity.Authenticator?.OnPageLoading(uri);

            Finish();
        }
    }
}
