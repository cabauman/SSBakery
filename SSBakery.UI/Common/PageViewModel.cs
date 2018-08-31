using GameCtor.RxNavigation;

namespace SSBakery.UI.Common
{
    public class PageViewModel : ViewModelBase, IPageViewModel
    {
        public PageViewModel(IViewStackService viewStackService)
            : base(viewStackService)
        {
        }

        public string Title => GetType().Name;
    }
}
