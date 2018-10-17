using ReactiveUI;
using SSBakeryAdmin.UI.Common;

namespace SSBakeryAdmin.UI.Modules
{
    public class StampCellViewModel : ViewModelBase, IStampCellViewModel
    {
        private bool _stamped;

        public StampCellViewModel(bool stamped)
        {
            Stamped = stamped;
        }

        public bool Stamped
        {
            get => _stamped;
            set => this.RaiseAndSetIfChanged(ref _stamped, value);
        }
    }
}
