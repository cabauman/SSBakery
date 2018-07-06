using System;
using System.Reactive;
using System.Reactive.Linq;
using ReactiveUI;
using Splat;

namespace SSBakery.UI.Common
{
    public class ViewModelBase : ReactiveObject, IRoutableViewModel, ISupportsActivation
    {
        public ViewModelBase(IScreen hostScreen = null)
        {
            HostScreen = hostScreen ?? Locator.Current.GetService<IScreen>();
        }

        public string UrlPathSegment
        {
            get;
            protected set;
        }

        public IScreen HostScreen
        {
            get;
            protected set;
        }

        public ViewModelActivator Activator { get; } = new ViewModelActivator();

        protected IObservable<Unit> Navigate(IRoutableViewModel routableViewModel)
        {
            return HostScreen
                .Router
                .Navigate
                .Execute(routableViewModel)
                .Select(_ => Unit.Default);
        }

        protected IObservable<Unit> NavigateAndReset(IRoutableViewModel routableViewModel)
        {
            return HostScreen
                .Router
                .NavigateAndReset
                .Execute(routableViewModel)
                .Select(_ => Unit.Default);
        }
    }
}
