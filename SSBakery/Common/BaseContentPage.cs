using ReactiveUI.XamForms;

namespace SSBakery.Common
{
    public class BaseContentPage<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : class
    {
    }
}
