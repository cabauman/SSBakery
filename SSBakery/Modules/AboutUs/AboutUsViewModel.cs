using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Modules
{
    public class AboutUsViewModel : PageViewModel, IAboutUsViewModel
    {
        public AboutUsViewModel(IViewStackService viewStackService = null)
            : base(viewStackService)
        {
        }
    }
}
