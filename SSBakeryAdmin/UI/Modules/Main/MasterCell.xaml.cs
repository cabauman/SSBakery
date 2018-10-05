using System;
using System.Reactive.Linq;
using ReactiveUI;
using ReactiveUI.XamForms;

namespace SSBakeryAdmin.UI.Modules
{
    public partial class MasterCell : ReactiveViewCell<MasterCellViewModel>
    {
        public MasterCell()
        {
            InitializeComponent();

            this.WhenAnyValue(x => x.ViewModel)
                .Where(x => x != null)
                .Do(PopulateFromViewModel)
                .Subscribe();
        }

        private void PopulateFromViewModel(MasterCellViewModel viewModel)
        {
            TitleLabel.Text = viewModel.Title;
            IconImage.Source = viewModel.IconSource;
        }
    }
}
