using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class HoursAndLocationViewModel : ViewModelBase
    {
        public HoursAndLocationViewModel(IScreen hostScreen = null)
            : base(hostScreen)
        {
        }
    }
}
