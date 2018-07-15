using SSBakery.UI.Navigation.Interfaces;

namespace SSBakery.UI.Common
{
    public class PageViewModel : ViewModelBase, IPageViewModel
    {
        public PageViewModel(IViewStackService viewStackService)
            : base(viewStackService)
        {
        }

        public string Id => GetType().Name;
    }
}
