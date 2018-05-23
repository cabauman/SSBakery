using System;
using System.Reactive.Disposables;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class MainViewModel : ViewModelBase
    {
        public MainViewModel(IScreen hostScreen = null)
            : base(hostScreen)
        {
        }
    }
}
