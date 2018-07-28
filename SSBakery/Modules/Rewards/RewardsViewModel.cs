﻿using SSBakery.UI.Common;
using SSBakery.UI.Navigation.Interfaces;

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
