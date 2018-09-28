using GameCtor.RxNavigation;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class StoreInfoViewModel : PageViewModel, IStoreInfoViewModel
    {
        public StoreInfoViewModel(IViewStackService viewStackService = null)
            : base(viewStackService)
        {
        }
    }
}
