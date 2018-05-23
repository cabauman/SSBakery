using System;
using ReactiveUI.XamForms;

namespace SSBakery.UI.Common
{
    public class ContentPageBase<TViewModel> : ReactiveContentPage<TViewModel>
        where TViewModel : class
    {
    }
}
