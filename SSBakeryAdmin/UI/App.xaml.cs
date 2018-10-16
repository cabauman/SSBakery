using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ReactiveUI;
using SSBakery.Config;
using SSBakeryAdmin.UI.Common;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace SSBakeryAdmin
{
    public partial class App : ReactiveApplication<AppBootstrapper>
    {
        public App()
        {
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense(Constants.SYNC_FUSION_LICENSE);

            InitializeComponent();

            ViewModel = new AppBootstrapper();
            Task.Run(async () => { await ViewModel.NavigateToFirstPage(); }).Wait();
            this.OneWayBind(ViewModel, vm => vm.MainView, v => v.MainPage, selector: ResolvePage);
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

        private Page ResolvePage(object viewModel)
        {
            var viewFor = ViewLocator.Current.ResolveView(viewModel);
            var page = viewFor as Page;
            if(page == null)
            {
                throw new InvalidOperationException($"Resolved view '{viewFor.GetType().FullName}' for type '{viewModel.GetType().FullName}', is not a Page.");
            }

            viewFor.ViewModel = viewModel;

            return page;
        }
    }
}
