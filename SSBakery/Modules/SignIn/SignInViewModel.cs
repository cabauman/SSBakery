using System;
using System.Reactive;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;
using SSBakery;
using SSBakery.UI.Common;

namespace SSBakery.UI.Modules
{
    public class SignInViewModel : ViewModelBase
    {
        public SignInViewModel(IScreen hostScreen = null)
            : base(hostScreen)
        {
        }

        public ReactiveCommand GoToMainPage { get; }

        public IObservable<Unit> GoToPage(IRoutableViewModel routableViewModel)
        {
            return HostScreen
                .Router
                .Navigate
                .Execute(routableViewModel)
                .Select(_ => Unit.Default);
        }
    }
}
