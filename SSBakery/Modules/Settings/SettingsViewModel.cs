using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

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
