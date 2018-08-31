using RxNavigation;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class SettingsViewModel : PageViewModel, ISettingsViewModel
    {
        public SettingsViewModel(IViewStackService viewStackService = null)
            : base(viewStackService)
        {
        }
    }
}
