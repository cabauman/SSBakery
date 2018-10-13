using System;
using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class RewardsMemberRepo : FirebaseOfflineRepo<RewardsMember>, IRewardsMemberRepo
    {
        public RewardsMemberRepo(FirebaseClient client, string path, string key = null)
            : base(client, path, key)
        {
        }
    }
}
