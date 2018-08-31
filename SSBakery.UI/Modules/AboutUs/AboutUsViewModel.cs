using RxNavigation;
using SSBakery.UI.Common;

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
