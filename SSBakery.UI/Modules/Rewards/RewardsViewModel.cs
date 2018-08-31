using RxNavigation;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class RewardsViewModel : PageViewModel, IRewardsViewModel
    {
        public RewardsViewModel(IViewStackService viewStackService = null)
            : base(viewStackService)
        {
        }
    }
}
