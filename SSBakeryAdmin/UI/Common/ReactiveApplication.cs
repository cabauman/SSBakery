using ReactiveUI;
using Xamarin.Forms;

namespace SSBakeryAdmin.UI.Common
{
    public class ReactiveApplication<TViewModel> : Application, IViewFor<TViewModel>
        where TViewModel : class
    {
        public TViewModel ViewModel { get; set; }

        object IViewFor.ViewModel
        {
            get => ViewModel;
            set => ViewModel = (TViewModel)value;
        }
    }
}
