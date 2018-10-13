using System;
using System.Reactive;
using DynamicData;
using SSBakery.Models;

namespace SSBakery.Helpers
{
    public interface IRewardsMemberSynchronizer
    {
        IObservable<Unit> PullFromPosSystemAndStoreInFirebase(ISourceCache<RewardsMember, string> rewardsMemberCache);
    }
}
