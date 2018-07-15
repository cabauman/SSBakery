using ReactiveUI;
using Splat;
using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Common
{
    public class ViewModelBase : ReactiveObject, ISupportsActivation, IEnableLogger
    {
        private ViewModelActivator _activator;

        public ViewModelBase(IViewStackService viewStackService = null)
        {
            ViewStackService = viewStackService ?? Locator.Current.GetService<IViewStackService>();
        }

        public ViewModelActivator Activator
        {
            get
            {
                _activator = _activator ?? new ViewModelActivator();

                return _activator;
            }
        }

        protected IViewStackService ViewStackService { get; }
    }
}
