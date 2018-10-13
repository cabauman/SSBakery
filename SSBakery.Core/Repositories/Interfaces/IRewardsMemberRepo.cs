using System;
using GameCtor.Repository;
using SSBakery.Models;

namespace SSBakery.Repositories.Interfaces
{
    public interface IRewardsMemberRepo : IRepository<RewardsMember>
    {
        IObservable<FirebaseEvent<RewardsMember>> Observe();
    }
}
