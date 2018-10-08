using System;
using System.Reactive.Linq;
using Firebase.Database;
using GameCtor.FirebaseDatabase.DotNet;
using SSBakery.Models;
using SSBakery.Repositories.Interfaces;

namespace SSBakery.Repositories
{
    public class AdminVarRepo : FirebaseOfflineRepo<AdminVar>, IAdminVarRepo
    {
        private string _childNode;

        public AdminVarRepo(FirebaseClient client, string path, string childNode)
            : base(client, path)
        {
            _childNode = childNode;
        }

        public IObservable<AdminVar> GetInstance()
        {
            return GetItem(_childNode)
                .Select(x => x ?? new AdminVar());
        }
    }
}
