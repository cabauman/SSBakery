using System;
using System.Reactive.Disposables;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace SSBakeryAdmin.UI.Modules
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StampCell : ReactiveContentView<IStampCellViewModel>
    {
        private const double NO_STAMP_OPACITY = 0.25d;

        public StampCell()
        {
            InitializeComponent();

            this.WhenActivated(
                disposables =>
                {
                    Console.WriteLine(StampImage.Width);
                    this
                        .OneWayBind(ViewModel, vm => vm.Stamped, v => v.StampImage.Opacity, x => x ? 1d : NO_STAMP_OPACITY)
                        .DisposeWith(disposables);
                });
        }
    }
}
