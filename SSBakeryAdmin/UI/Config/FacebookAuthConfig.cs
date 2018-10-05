namespace SSBakeryAdmin.Config
{
    public static class FacebookAuthConfig
    {
        public const string SCOPE = "email";
        public const string CLIENT_ID = "<CLIENT ID>";
        public const string DATA_SCHEME = "fb" + CLIENT_ID;
        public const string REDIRECT_URL = DATA_SCHEME + "://authorize";
        public const string AUTHORIZE_URL = "https://www.facebook.com/dialog/oauth/";
    }
}
