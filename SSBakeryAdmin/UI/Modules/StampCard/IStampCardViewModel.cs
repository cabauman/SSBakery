using System.Collections.Generic;
using System.Reactive;
using ReactiveUI;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IStampCardViewModel
    {
        int RewardCount { get; set; }

        IStampCellViewModel SelectedItem { get; set; }

        IReadOnlyList<IStampCellViewModel> StampCells { get; }

        ReactiveCommand<Unit, Unit> Save { get; }

        ReactiveCommand<Unit, Unit> IncrementRewardCount { get; }

        ReactiveCommand<Unit, Unit> DecrementRewardCount { get; }

        ReactiveCommand<Unit, Unit> StartNewStampCard { get; }
    }
}
