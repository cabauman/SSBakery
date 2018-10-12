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
    public partial class RewardsMemberCell : ReactiveViewCell<IRewardsMemberCellViewModel>
    {
        public RewardsMemberCell()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateFromViewModel)
                .Subscribe();
        }

        private void PopulateFromViewModel(IRewardsMemberCellViewModel viewModel)
        {
            DescriptionLabel.Text = viewModel.TotalVisits.ToString();
            NameLabel.Text = viewModel.Name;
        }
    }
}
