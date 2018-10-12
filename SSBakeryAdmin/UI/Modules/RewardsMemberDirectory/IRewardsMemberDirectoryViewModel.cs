using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Reactive;
using System.Reactive.Subjects;
using DynamicData;
using GameCtor.Repository;
using ReactiveUI;
using SSBakery.Models;

namespace SSBakeryAdmin.UI.Modules
{
    public interface IRewardsMemberDirectoryViewModel
    {
        ReadOnlyObservableCollection<IRewardsMemberCellViewModel> MemberCells { get; }

        IConnectableObservable<FirebaseEvent<RewardsMember>> WhenRewardsMembersModified { get; }

        ReactiveCommand<Unit, Unit> SyncWithPosSystem { get; }

        ReactiveCommand<Unit, Unit> LoadRewardsMembers { get; }

        ReactiveCommand<IRewardsMemberCellViewModel, Unit> NavigateToRewardsMember { get; }

        //void AddUnclaimedReward(int index);

        //void RemoveUnclaimedReward(int index);
    }
}
