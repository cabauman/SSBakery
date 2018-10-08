using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IRewardsMemberDirectoryViewModel
    {
        ReadOnlyObservableCollection<IRewardsMemberCellViewModel> CustomersCells { get; }

        ReactiveCommand<Unit, IReadOnlyList<RewardsMember>> LoadCustomers { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<IRewardsMemberCellViewModel, Unit> NavigateToCustomer { get; }

        IRewardsMemberCellViewModel CellDisappearing { get; }

        //void AddUnclaimedReward(int index);

        //void RemoveUnclaimedReward(int index);
    }
}
