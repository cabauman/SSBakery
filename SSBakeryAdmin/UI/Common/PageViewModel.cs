using GameCtor.RxNavigation;

namespace SSBakeryAdmin.UI.Common
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
