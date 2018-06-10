namespace SSBakery.Config
{
    public static class GoogleAuthConfig
    {
        public const string SCOPE = "email";
        public const string AUTHORIZE_URL = "https://accounts.google.com/o/oauth2/v2/auth";
        public const string ACCESS_TOKEN_URL = "https://www.googleapis.com/oauth2/v4/token";

        public const string CLIENT_ID_IOS = "<iOS CLIENT ID>.apps.googleusercontent.com";
        public const string REDIRECT_URL_IOS = "<BUNDLE ID>:/oauth2redirect";

#if DEBUG
        public const string CLIENT_ID_ANDROID = "<DEBUG ANDROID CLIENT ID>.apps.googleusercontent.com";
        public const string DATA_SCHEME_ANDROID = "com.googleusercontent.apps.<DEBUG ANDROID CLIENT ID>";
#else
        public const string CLIENT_ID_ANDROID = "<RELEASE ANDROID CLIENT ID>.apps.googleusercontent.com";
        public const string DATA_SCHEME_ANDROID = "com.googleusercontent.apps.<RELEASE ANDROID CLIENT ID>";
#endif
        public const string REDIRECT_URL_ANDROID = DATA_SCHEME_ANDROID + ":/oauth2redirect";
    }
}