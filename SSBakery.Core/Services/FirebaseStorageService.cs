using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using SSBakery.Services.Interfaces;

namespace SSBakery.Services
{
    public class FirebaseStorageService : IFirebaseStorageService
    {
        private const string URL = "";

        public IObservable<string> GetDownloadUrl(string filename)
        {
            throw new NotImplementedException();
        }
    }
}
