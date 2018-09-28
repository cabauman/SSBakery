using System;
using Foundation;
using GameCtor.FirebaseAuth;
using GameCtor.FirebaseAuth.Mobile;
using GameCtor.XamarinAuth;
using ReactiveUI;
using GameCtor.RxNavigation;
using Splat;
using SSBakeryAdmin.iOS.Modules.CatalogCategory;
using SSBakeryAdmin.iOS.Modules.CatalogCategoryList;
using SSBakeryAdmin.iOS.Modules.CustomerDirectory;
using SSBakeryAdmin.iOS.Modules.Home;
using SSBakeryAdmin.iOS.Modules.SignIn;
using SSBakeryAdmin.Services;
using SSBakeryAdmin.Services.Interfaces;
using UIKit;

namespace SSBakeryAdmin.iOS
{
    // The UIApplicationDelegate for the application. This class is responsible for launching the
    // User Interface of the application, as well as listening (and optionally responding) to application events from iOS.
    [Register("AppDelegate")]
    public class AppDelegate : UIApplicationDelegate
    {
        // class-level declarations

        public override UIWindow Window
        {
            get;
            set;
        }

        public override bool FinishedLaunching(UIApplication application, NSDictionary launchOptions)
        {
            Firebase.Core.App.Configure();
            Xamarin.Auth.Presenters.XamarinIOS.AuthenticationConfiguration.Init();

            // create a new window instance based on the screen size
            Window = new UIWindow(UIScreen.MainScreen.Bounds);

            var mainView = new MainView(RxApp.TaskpoolScheduler, RxApp.MainThreadScheduler, ViewLocator.Current);
            var viewStackService = new ViewStackService(mainView);
            RegisterViews(viewStackService);
            viewStackService.PushPage(new SignInViewModel()).Subscribe();

            Window.RootViewController = mainView;
            Window.MakeKeyAndVisible();

            return true;
        }

        public override bool OpenUrl(UIApplication app, NSUrl url, NSDictionary options)
        {
            // Convert NSUrl to Uri
            var uri = new Uri(url.AbsoluteString);

            // Load redirectUrl page
            AuthenticationState.Authenticator.OnPageLoading(uri);

            return true;
        }

        public override void OnResignActivation(UIApplication application)
        {
            // Invoked when the application is about to move from active to inactive state.
            // This can occur for certain types of temporary interruptions (such as an incoming phone call or SMS message) 
            // or when the user quits the application and it begins the transition to the background state.
            // Games should use this method to pause the game.
        }

        public override void DidEnterBackground(UIApplication application)
        {
            // Use this method to release shared resources, save user data, invalidate timers and store the application state.
            // If your application supports background exection this method is called instead of WillTerminate when the user quits.
        }

        public override void WillEnterForeground(UIApplication application)
        {
            // Called as part of the transiton from background to active state.
            // Here you can undo many of the changes made on entering the background.
        }

        public override void OnActivated(UIApplication application)
        {
            // Restart any tasks that were paused (or not yet started) while the application was inactive. 
            // If the application was previously in the background, optionally refresh the user interface.
        }

        public override void WillTerminate(UIApplication application)
        {
            // Called when the application is about to terminate. Save data, if needed. See also DidEnterBackground.
        }

        private void RegisterViews(IViewStackService viewStackService)
        {
            Locator.CurrentMutable.RegisterConstant(viewStackService, typeof(IViewStackService));
            Locator.CurrentMutable.RegisterConstant(new AuthService(), typeof(IAuthService));
            Locator.CurrentMutable.RegisterConstant(new FirebaseAuthService(), typeof(IFirebaseAuthService));

            Locator.CurrentMutable.Register(() => new SignInController(), typeof(IViewFor<ISignInViewModel>));
            Locator.CurrentMutable.Register(() => new HomeController(), typeof(IViewFor<IHomeViewModel>));
            //Locator.CurrentMutable.Register(() => new CustomerDirectoryController(), typeof(IViewFor<ICustomerDirectoryViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryListController(), typeof(IViewFor<ICatalogCategoryListViewModel>));
            Locator.CurrentMutable.Register(() => new CatalogCategoryController(), typeof(IViewFor<ICatalogCategoryViewModel>));
        }
    }
}


